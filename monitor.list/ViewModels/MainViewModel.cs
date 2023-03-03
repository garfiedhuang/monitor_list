using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Xp.Resin.Print.Models;
using Xp.Resin.Print.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Threading;
using HandyControl.Controls;
using HandyControl.Data;

namespace Xp.Resin.Print.ViewsModels
{
    public class MainViewModel : ViewModelBase
    {
        private static string _msg = "";
        private static List<ConfigModel> _configModels;

        private static CancellationTokenSource tokenSource = new CancellationTokenSource();
        private static CancellationToken token = tokenSource.Token;

        /// <summary>
        /// ���캯��
        /// </summary>
        public MainViewModel()
        {
            try
            {
                BaseModel = new BaseModel();
                PageModel = new PageModel();
                DetailGridData = new ObservableCollection<DetailModel>();
                DetailModels = new ObservableCollection<DetailModel>();
                SummaryGridData = new ObservableCollection<SummaryModel>();

                this.QueryCmd = new RelayCommand(this.Query);
                this.ResetCmd = new RelayCommand(this.Reset);

                this.PageUpdatedCommand = new RelayCommand<FunctionEventArgs<int>>(PageUpdated);

                //������ݿ��ļ�
                OleDbHelper.CheckDbFile();

                //��������
                this.GetConfigs();

                //��ʼ��ҳ������
                this.Query();

                #region �첽�߳�

                var enableExecuteMonitorJob = Convert.ToInt32(_configModels.FirstOrDefault(w => w.Category == "SYSTEM" && w.ConfigKey == "ENABLE_EXECUTE_PRINT_JOB")?.ConfigValue ?? "1");

                //ִ�д�ӡ�߳�
                if (enableExecuteMonitorJob == 1)
                {
                    _ = Task.Factory.StartNew(() =>
                    {
                        DoExecuteMonitor();
                    }, token);
                }

                #endregion

            }
            catch (Exception ex)
            {
                LogHelper.Error($"��ʼ��ʧ�ܣ�{ex?.Message}");
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        ~MainViewModel()
        {
            tokenSource.Cancel();
        }

        #region =====data

        /// <summary>
        /// ��ѯ
        /// </summary>
        private BaseModel baseModel;
        public BaseModel BaseModel
        {
            get { return baseModel; }
            set { baseModel = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// ��ҳ
        /// </summary>
        private PageModel pageModel;
        public PageModel PageModel
        {
            get { return pageModel; }
            set { pageModel = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// ��ϸ��������
        /// </summary>
        private ObservableCollection<DetailModel> detailGridData;
        public ObservableCollection<DetailModel> DetailGridData
        {
            get { return detailGridData; }
            set { detailGridData = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// ������ϸ
        /// </summary>
        private ObservableCollection<DetailModel> detailModels;
        public ObservableCollection<DetailModel> DetailModels
        {
            get { return detailModels; }
            set { detailModels = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// ������������
        /// </summary>
        private ObservableCollection<SummaryModel> summaryGridData;
        public ObservableCollection<SummaryModel> SummaryGridData
        {
            get { return summaryGridData; }
            set { summaryGridData = value; RaisePropertyChanged(); }
        }

        #endregion

        #region ====cmd

        public RelayCommand QueryCmd { get; set; }
        public RelayCommand ResetCmd { get; set; }

        /// <summary>
        /// ��ҳ
        /// </summary>
        public RelayCommand<FunctionEventArgs<int>> PageUpdatedCommand { get; set; }

        #endregion

        #region =====implement

        private void GetConfigs()
        {
            try
            {
                if (_configModels == null)
                {
                    _configModels = new List<ConfigModel>();
                }

                var sql = $"SELECT * FROM BASE_CONFIG WHERE 1=1 AND IS_DELETED=0";

                using (var dt = OleDbHelper.DataTable(sql))
                {

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return;
                    }

                    foreach (DataRow dr in dt.Rows)
                    {
                        _configModels.Add(new ConfigModel()
                        {
                            Id = Convert.ToInt32(dr["ID"]),
                            Category = dr["CATEGORY"].ToString(),
                            ConfigKey = dr["CONFIG_KEY"].ToString(),
                            ConfigValue = dr["CONFIG_VALUE"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _msg = $"��ѯ����ʧ�ܣ�{ex?.Message}";

                Growl.Error(_msg);
                LogHelper.Error(_msg);
            }
        }

        /// <summary>
        /// ��ѯ
        /// </summary>
        public void Query()
        {
            try
            {
                var conditions = new StringBuilder();

                if (!string.IsNullOrEmpty(BaseModel.Name.Trim()))
                {
                    conditions.Append($" AND NAME LIKE '%{BaseModel.Name}%'");
                }

                if (!string.IsNullOrEmpty(BaseModel.Unit.Trim()))
                {
                    conditions.Append($" AND UNIT LIKE '%{BaseModel.Unit}%'");
                }

                if (!string.IsNullOrEmpty(BaseModel.Result.Trim()))
                {
                    conditions.Append($" AND RESULT LIKE '%{BaseModel.Result}%'");
                }

                if (!string.IsNullOrEmpty(BaseModel.ApprovalUnit.Trim()))
                {
                    conditions.Append($" AND APPROVAL_UNIT LIKE '%{BaseModel.ApprovalUnit}%'");
                }

                if (!string.IsNullOrEmpty(BaseModel.NoticeDate.Trim()))
                {
                    conditions.Append($" AND NOTICE_DATE LIKE '%{BaseModel.NoticeDate}%'");
                }

                var sql = $"SELECT * FROM MONITOR_LIST WHERE 1=1{conditions} ORDER BY LAST_PRINT_DT DESC";

                using (var dt = OleDbHelper.DataTable(sql))
                {
                    DetailModels.Clear();
                    DetailGridData.Clear();

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        return;
                    }

                    var rowNum = 1;//�к�

                    foreach (DataRow dr in dt.Rows)
                    {
                        DetailModels.Add(new DetailModel()
                        {
                            Id = Convert.ToInt32(dr["ID"]),
                            RowNum = rowNum,
                            Name = dr["NAME"].ToString(),
                            Unit = dr["UNIT"].ToString(),
                            Result = dr["RESULT"].ToString(),
                            ApprovalUnit = dr["APPROVAL_UNIT"].ToString(),
                            NoticeStartTime = GetDateTime(dr["NOTICE_START_TIME"]),
                            NoticeEndTime = GetDateTime(dr["NOTICE_END_TIME"])
                        });

                        rowNum++;
                    }
                }

                //��ǰҳ��
                PageModel.PageIndex = DetailModels.Count > 0 ? 1 : 0;
                PageModel.MaxPageCount = 0;

                //���ҳ��
                PageModel.MaxPageCount = PageModel.PageIndex > 0 ? (int)Math.Ceiling((decimal)DetailModels.Count / PageModel.DataCountPerPage) : 0;

                //���ݷ�ҳ
                Paging(PageModel.PageIndex);

            }
            catch (Exception ex)
            {
                _msg = $"��ѯʧ�ܣ�{ex?.Message}";

                Growl.Error(_msg);
                LogHelper.Error(_msg);
            }
        }

        private DateTime GetDateTime(object obj)
        {
            if (obj is DBNull)
            {
                return DateTime.MinValue;
            }
            else
            {
                return Convert.ToDateTime(obj);
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        private void Reset()
        {
            this.BaseModel.Name = string.Empty;
            this.BaseModel.Unit = string.Empty;
            this.BaseModel.Result = string.Empty;
            this.BaseModel.ApprovalUnit = string.Empty;
            this.BaseModel.NoticeDate = string.Empty;
        }

        private void SetPrintTips(int v)
        {
            if (BaseModel.MonitorTaskCount < 0)
            {
                BaseModel.MonitorTaskCount = 0;
            }
            else
            {
                BaseModel.MonitorTaskCount += v;//��ӡ����������
            }

            if (BaseModel.MonitorTaskCount == 0)
            {
                BaseModel.PrintTaskTips = string.Empty;
            }
            else
            {
                BaseModel.PrintTaskTips = $"����{BaseModel.MonitorTaskCount}����ӡ��";//���õ�ǰ��ӡ��������
            }
        }

        /// <summary>
        /// ��ҳ
        /// </summary>
        /// <param name="obj"></param>
        private void PageUpdated(FunctionEventArgs<int> obj)
        {
            Paging(obj.Info);
        }

        /// <summary>
        /// ��ҳ
        /// </summary>
        /// <param name="pageIndex"></param>
        private void Paging(int pageIndex = 0)
        {
            if (pageIndex == -1)//��һ�β�ѯ�������ò�ѯ
            {
                //��ǰҳ��
                PageModel.PageIndex = DetailModels.Count > 0 ? 1 : 0;
                PageModel.MaxPageCount = 0;

                pageIndex = PageModel.PageIndex;
            }

            //���ҳ��
            PageModel.MaxPageCount = PageModel.PageIndex > 0 ? (int)Math.Ceiling((decimal)DetailModels.Count / PageModel.DataCountPerPage) : 0;

            //�����������
            DetailGridData.Clear();

            //���ݷ�ҳ
            var pagedData = DetailModels.Skip((pageIndex - 1) * PageModel.DataCountPerPage).Take(PageModel.DataCountPerPage).ToList();

            if (pagedData.Count > 0)
            {
                pagedData.ForEach(item =>
                {
                    DetailGridData.Add(item);
                });
            }

            //���ܱ�ֵ
            if (SummaryGridData != null)
            {
                SummaryGridData.Clear();
            }

            if (DetailModels != null && DetailModels.Count > 0)
            {
                var groupData = DetailModels.GroupBy(p => p, new DetailModelEqualityComparer());

                var rowCount = 1;

                foreach (var group in groupData)
                {
                    //var key = group.Key.ToString().Split('-');

                    //var date = key[0];
                    //var approvalUnit = key[1];

                    SummaryGridData.Add(new SummaryModel()
                    {

                        Id = group.Key.GetHashCode(),
                        RowNum = rowCount,
                        NoticeDate = group.Key.NoticeStartTime.ToString("yyyy/MM/dd"),
                        ApprovalUnit = group.Key.ApprovalUnit,
                        Summary = group.Count()

                    });

                    rowCount++;

                    /*
                    Console.WriteLine(group.Key.ToString());
                    foreach (var person in group)
                    {
                        Console.WriteLine($"\t{person.Name},{person.ApprovalUnit}");
                    }
                    */
                }
            }
        }

        #endregion

        #region threads


        private void DoExecuteMonitor()
        {
            var sql = "SELECT * FROM TS_PRINT_QUEUE WHERE PRINT_DT IS NULL ORDER BY CREATE_DT ASC";

            var msg = string.Empty;
            var printParams = new PrintParams();
            var zebraCommand = string.Empty;
            var printResult = false;
            var jobInterval = Convert.ToInt32(_configModels.FirstOrDefault(w => w.Category == "SYSTEM" && w.ConfigKey == "EXECUTE_PRINT_JOB_INTERVAL")?.ConfigValue ?? "3");

            while (true)
            {
                try
                {
                    if (token.IsCancellationRequested)
                    {
                        break;//ȡ��Taskִ��
                    }

                    using (var data = OleDbHelper.DataTable(sql))
                    {
                        if (data == null || data.Rows.Count == 0)
                        {
                            continue;
                        }
                        foreach (DataRow dr in data.Rows)
                        {
                            try
                            {
                                printResult = false;

                                //��ӡ����
                                printParams = Newtonsoft.Json.JsonConvert.DeserializeObject<PrintParams>(dr["PRINT_PARAMS"].ToString());

                                //��ӡָ��
                                zebraCommand = _configModels.FirstOrDefault(w => w.Category == "COMMAND" && w.ConfigKey == printParams.Printer)?.ConfigValue;

                                if (string.IsNullOrEmpty(zebraCommand))
                                {
                                    throw new Exception($"��ӡ����{printParams.Printer}��δ���ô�ӡָ�����ϵIT����Ա��");
                                }

                                //��ʽ����ӡָ��
                                zebraCommand = zebraCommand.Replace("#qrMaterialNo#", printParams.MaterialNo)
                                    .Replace("#txtCarType#", printParams.CarType)
                                    .Replace("#txtMaterialName#", printParams.MaterialName)
                                    .Replace("#txtSupplier#", printParams.SupplierName)
                                    .Replace("#txtProductDate#", printParams.ProductDate.ToString("yyyy/MM/dd", DateTimeFormatInfo.InvariantInfo))
                                    .Replace("#copies#", printParams.Copies.ToString());

                                var temp1 = ZebraPrintHelper.ConvertChineseToHex(printParams.MaterialName, "tempName1");
                                var temp2 = ZebraPrintHelper.ConvertChineseToHex(printParams.SupplierName, "tempName2");

                                zebraCommand = zebraCommand.Replace("{tempName1}", temp1).Replace("{tempName2}", temp2);

                                //���Ͱ����ӡ��
                                //printResult = RawPrinterHelper.SendStringToPrinter(printParams.Printer, zebraCommand);

                                msg = $"��ӡ��{printParams.CarType}|{printParams.MaterialNo}|{printParams.MaterialName}��";

                                if (printResult)
                                {
                                    msg = $"{msg}������ͳɹ���";
                                    Growl.Info(msg);
                                    LogHelper.Info($"����ͳɹ�����ӡ������{dr["PRINT_PARAMS"]}");
                                }
                                else
                                {
                                    msg = $"{msg}�������ʧ�ܣ�";
                                    Growl.Error(msg);
                                    LogHelper.Error(msg);
                                }
                            }
                            catch (Exception ex)
                            {
                                msg = $"���л���ӡ��������ָ���������ӡ��ʧ�ܣ�{ex?.Message}";

                                Growl.Error(msg);
                                LogHelper.Error(msg);
                            }

                            finally
                            {
                                //���´�ӡ״̬ 
                                try
                                {
                                    var sqls = new ArrayList();

                                    sqls.Add($"UPDATE TS_PRINT_QUEUE SET STATUS='{(printResult?1:0)}',REMARK='{msg}',PRINT_DT=NOW(),UPDATE_DT=NOW() WHERE ID={dr["ID"]}");
                                    sqls.Add($"UPDATE BASE_PRODUCT SET LAST_PRINT_DT=NOW(),UPDATE_DT=NOW() WHERE CAR_TYPE='{printParams.CarType}' AND MATERIAL_NO='{printParams.MaterialNo}' AND IS_DELETED=0");

                                    OleDbHelper.ExecuteSqlTran(sqls);

                                    //ˢ�½�������״̬
                                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                                    {
                                        this.Query();
                                    });
                                }
                                catch (Exception ex)
                                {
                                    msg = $"���´�ӡ״̬��{ex?.Message}";

                                    Growl.Error(msg);
                                    LogHelper.Error(msg);
                                }

                                this.SetPrintTips(-1);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    msg = $"ִ�д�ӡJOBʧ�ܣ�{ex?.Message}";

                    Growl.Error(msg);
                    LogHelper.Error(msg);
                }
                finally
                {
                    Thread.Sleep(jobInterval * 1000);//Ĭ������3s
                }
            }
        }

        #endregion
    }

    class DetailModelEqualityComparer : IEqualityComparer<DetailModel>
    {
        public bool Equals(DetailModel x, DetailModel y) => x.NoticeStartTime.ToString("yyyy/MM/dd") == y.NoticeStartTime.ToString("yyyy/MM/dd") && x.ApprovalUnit == y.ApprovalUnit;

        public int GetHashCode(DetailModel obj) => $"{obj.NoticeStartTime:yyyy/MM/dd}-{obj.ApprovalUnit}".GetHashCode();
    }
}