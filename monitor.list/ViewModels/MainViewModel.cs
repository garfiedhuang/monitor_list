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
using System.Text.RegularExpressions;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Collections.Concurrent;

namespace Xp.Resin.Print.ViewsModels {
    public class MainViewModel : ViewModelBase {
        private static string _msg = "";
        private static List<ConfigModel> _configModels;

        private static CancellationTokenSource tokenSource = new CancellationTokenSource();
        private static CancellationToken token = tokenSource.Token;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainViewModel() {
            try {
                BaseModel = new BaseModel();
                PageModel = new PageModel();
                DetailGridData = new ObservableCollection<DetailModel>();
                DetailModels = new ObservableCollection<DetailModel>();
                SummaryGridData = new ObservableCollection<SummaryModel>();

                this.QueryCmd = new RelayCommand(this.Query);
                this.ResetCmd = new RelayCommand(this.Reset);

                this.PageUpdatedCommand = new RelayCommand<FunctionEventArgs<int>>(PageUpdated);

                //检查数据库文件
                OleDbHelper.CheckDbFile();

                //加载配置
                this.GetConfigs();

                //初始化页面数据
                this.Query();

                #region 异步线程

                var enableExecuteMonitorJob = Convert.ToInt32(_configModels.FirstOrDefault(w => w.Category == "SYSTEM" && w.ConfigKey == "ENABLE_MONITOR_JOB")?.ConfigValue ?? "1");

                //执行打印线程
                if (enableExecuteMonitorJob == 1) {
                    _ = Task.Factory.StartNew(() => {
                        DoExecuteMonitor();
                    }, token);
                }

                #endregion

                //测试
                //GetPageContent();

            }
            catch (Exception ex) {
                LogHelper.Error($"初始化失败，{ex?.Message}");
            }
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~MainViewModel() {
            tokenSource.Cancel();
        }

        #region =====data

        /// <summary>
        /// 查询
        /// </summary>
        private BaseModel baseModel;
        public BaseModel BaseModel {
            get { return baseModel; }
            set { baseModel = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 分页
        /// </summary>
        private PageModel pageModel;
        public PageModel PageModel {
            get { return pageModel; }
            set { pageModel = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 明细网格数据
        /// </summary>
        private ObservableCollection<DetailModel> detailGridData;
        public ObservableCollection<DetailModel> DetailGridData {
            get { return detailGridData; }
            set { detailGridData = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 所有明细
        /// </summary>
        private ObservableCollection<DetailModel> detailModels;
        public ObservableCollection<DetailModel> DetailModels {
            get { return detailModels; }
            set { detailModels = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 汇总网格数据
        /// </summary>
        private ObservableCollection<SummaryModel> summaryGridData;
        public ObservableCollection<SummaryModel> SummaryGridData {
            get { return summaryGridData; }
            set { summaryGridData = value; RaisePropertyChanged(); }
        }

        #endregion

        #region ====cmd

        public RelayCommand QueryCmd { get; set; }
        public RelayCommand ResetCmd { get; set; }

        /// <summary>
        /// 分页
        /// </summary>
        public RelayCommand<FunctionEventArgs<int>> PageUpdatedCommand { get; set; }

        #endregion

        #region =====implement

        private void GetConfigs() {
            try {
                if (_configModels == null) {
                    _configModels = new List<ConfigModel>();
                }

                var sql = $"SELECT * FROM BASE_CONFIG WHERE 1=1 AND IS_DELETED=0";

                using (var dt = OleDbHelper.DataTable(sql)) {

                    if (dt == null || dt.Rows.Count == 0) {
                        return;
                    }

                    foreach (DataRow dr in dt.Rows) {
                        _configModels.Add(new ConfigModel() {
                            Id = Convert.ToInt32(dr["ID"]),
                            Category = dr["CATEGORY"].ToString(),
                            ConfigKey = dr["CONFIG_KEY"].ToString(),
                            ConfigValue = dr["CONFIG_VALUE"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex) {
                _msg = $"查询配置失败，{ex?.Message}";

                Growl.Error(_msg);
                LogHelper.Error(_msg);
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        public void Query() {
            try {
                var conditions = new StringBuilder();

                if (!string.IsNullOrEmpty(BaseModel.Name.Trim())) {
                    conditions.Append($" AND FULL_NAME LIKE '%{BaseModel.Name}%'");
                }

                if (!string.IsNullOrEmpty(BaseModel.Unit.Trim())) {
                    conditions.Append($" AND UNIT LIKE '%{BaseModel.Unit}%'");
                }

                if (!string.IsNullOrEmpty(BaseModel.Result.Trim())) {
                    conditions.Append($" AND RESULT LIKE '%{BaseModel.Result}%'");
                }

                if (!string.IsNullOrEmpty(BaseModel.ApprovalUnit.Trim())) {
                    conditions.Append($" AND APPROVAL_UNIT LIKE '%{BaseModel.ApprovalUnit}%'");
                }

                if (!string.IsNullOrEmpty(BaseModel.NoticeDate.Trim())) {
                    conditions.Append($" AND NOTICE_DATE LIKE '%{BaseModel.NoticeDate}%'");
                }

                var sql = $"SELECT * FROM MONITOR_LIST WHERE 1=1{conditions} ORDER BY CREATE_DT DESC";

                using (var dt = OleDbHelper.DataTable(sql)) {
                    DetailModels.Clear();
                    DetailGridData.Clear();

                    if (dt == null || dt.Rows.Count == 0) {
                        return;
                    }

                    var rowNum = 1;//行号

                    foreach (DataRow dr in dt.Rows) {
                        DetailModels.Add(new DetailModel() {
                            Id = Convert.ToInt32(dr["ID"]),
                            RowNum = rowNum,
                            Name = dr["FULL_NAME"].ToString(),
                            Unit = dr["UNIT"].ToString(),
                            Result = dr["RESULT"].ToString(),
                            ApprovalUnit = dr["APPROVAL_UNIT"].ToString(),
                            NoticeStartTime = GetDateTime(dr["NOTICE_START_TIME"]),
                            NoticeEndTime = GetDateTime(dr["NOTICE_END_TIME"]),
                            CreateDt = GetDateTime(dr["CREATE_DT"]),
                        });

                        rowNum++;
                    }
                }

                //当前页数
                PageModel.PageIndex = DetailModels.Count > 0 ? 1 : 0;
                PageModel.MaxPageCount = 0;

                //最大页数
                PageModel.MaxPageCount = PageModel.PageIndex > 0 ? (int)Math.Ceiling((decimal)DetailModels.Count / PageModel.DataCountPerPage) : 0;

                //数据分页
                Paging(PageModel.PageIndex);

            }
            catch (Exception ex) {
                _msg = $"查询失败，{ex?.Message}";

                Growl.Error(_msg);
                LogHelper.Error(_msg);
            }
        }

        private DateTime GetDateTime(object obj) {
            if (obj is DBNull) {
                return DateTime.MinValue;
            }
            else {
                return Convert.ToDateTime(obj);
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        private void Reset() {
            this.BaseModel.Name = string.Empty;
            this.BaseModel.Unit = string.Empty;
            this.BaseModel.Result = string.Empty;
            this.BaseModel.ApprovalUnit = string.Empty;
            this.BaseModel.NoticeDate = string.Empty;
        }

        private void SetTips(string v) {
            BaseModel.DataTips = v;
            BaseModel.SummaryTips = $"总共采集 {detailModels.Count} 个";
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="obj"></param>
        private void PageUpdated(FunctionEventArgs<int> obj) {
            Paging(obj.Info);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="pageIndex"></param>
        private void Paging(int pageIndex = 0) {
            if (pageIndex == -1)//第一次查询或者重置查询
            {
                //当前页数
                PageModel.PageIndex = DetailModels.Count > 0 ? 1 : 0;
                PageModel.MaxPageCount = 0;

                pageIndex = PageModel.PageIndex;
            }

            //最大页数
            PageModel.MaxPageCount = PageModel.PageIndex > 0 ? (int)Math.Ceiling((decimal)DetailModels.Count / PageModel.DataCountPerPage) : 0;

            //清空依赖属性
            DetailGridData.Clear();

            //数据分页
            var pagedData = DetailModels.Skip((pageIndex - 1) * PageModel.DataCountPerPage).Take(PageModel.DataCountPerPage).ToList();

            if (pagedData.Count > 0) {
                pagedData.ForEach(item => {
                    DetailGridData.Add(item);
                });
            }

            //汇总表赋值
            if (SummaryGridData != null) {
                SummaryGridData.Clear();
            }

            if (DetailModels != null && DetailModels.Count > 0) {
                var groupData = DetailModels.GroupBy(p => p, new DetailModelEqualityComparer());

                var rowCount = 1;

                foreach (var group in groupData) {
                    //var key = group.Key.ToString().Split('-');

                    //var date = key[0];
                    //var approvalUnit = key[1];

                    SummaryGridData.Add(new SummaryModel() {

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


        private void GetPageContent() {

            using (System.Net.WebClient WebClientObj = new System.Net.WebClient()) {
                System.Collections.Specialized.NameValueCollection PostVars = new System.Collections.Specialized.NameValueCollection();
                PostVars.Add("__VIEWSTATE", _viewState);
                PostVars.Add("__EVENTVALIDATION", "");
                PostVars.Add("__EVENTTARGET", "LinkButton1");
                PostVars.Add("__EVENTARGUMENT", "");

                if (Convert.ToInt32(_toPage) > _totalPages) {
                    _toPage = "0";
                    _totalPages = 0;
                }
                else {
                    _toPage = (Convert.ToInt32(_toPage) + 1).ToString();
                }

                PostVars.Add("ToPage", _toPage);


                WebClientObj.Headers.Add("ContentType", "application/x-www-form-urlencoded");
                try {
                    byte[] byte1 = WebClientObj.UploadValues("http://gzrsj.hrssgz.gov.cn/vsgzpiapp01/GZPI/Gateway/PersonIntroducePublicity.aspx", "POST", PostVars);
                    string result = Encoding.UTF8.GetString(byte1); //得到当前页面对应的html 文本字符串  
                    GetSourceValue(result);//得到当前页面对应的 __VIEWSTATE 等上面需要的信息，为抓取下一页面使用  
                }
                catch (Exception ex) {
                    Growl.Error(ex.Message);
                    LogHelper.Error(ex.Message);
                }
            }
        }

        private void GetSourceValue(string result) {
            if (!string.IsNullOrEmpty(result)) {
                var regViewState = new Regex("(?<=<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"__VIEWSTATE\" value=\")(.*?)(?=\" />)", RegexOptions.IgnoreCase);
                var ms = regViewState.Matches(result);
                _viewState = ms[0].Value;

                var regTotalPages = new Regex("(?<=共<span id=\"PageCount\">)(.*?)(?=</span>页)", RegexOptions.IgnoreCase);
                var ms2 = regTotalPages.Matches(result);
                _totalPages = Convert.ToInt32(ms2[0].Value);

                var regHtmlData = new Regex("(?<=</tr><tr class=\"List(.*?)\">)([\\s\\S]+?)(?=</tr>)", RegexOptions.None);
                var ms3 = regHtmlData.Matches(result);

                var temp =string.Empty;
                foreach (var data in ms3) {

                    temp = data.ToString().Trim().Replace(" class=\"text-center\"", "");

                    if (!_totalData.ContainsKey(temp)) {
                        _totalData.TryAdd(temp, temp);
                        _htmlData.Add(temp);
                    }
                    else {
                        continue;
                    }
                }
            }
        }

        private static string _viewState = "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";
        private static string _toPage = "0";
        private static int _totalPages = 0;
        private static List<string> _htmlData = new List<string>();
        private static ConcurrentDictionary<string, string> _totalData=new ConcurrentDictionary<string, string>();

        private void DoExecuteMonitor() {

            var msg = string.Empty;

            var fullName = string.Empty;
            var unit = string.Empty;
            var approvalResult = string.Empty;
            var approvalUnit= string.Empty;
            var noticeStartTime = DateTime.MinValue;
            var noticeEndTime = DateTime.MinValue;

            var jobInterval = Convert.ToInt32(_configModels.FirstOrDefault(w => w.Category == "SYSTEM" && w.ConfigKey == "MONITOR_JOB_INTERVAL")?.ConfigValue ?? "3");

            while (true) {
                try {
                    if (token.IsCancellationRequested) {
                        break;//取消Task执行
                    }

                    //获取所有分页数据
                    _htmlData.Clear();
                    while (Convert.ToInt32(_toPage) == 0 || Convert.ToInt32(_toPage) <= _totalPages) {
                        GetPageContent();
                    }

                    if (_htmlData == null || _htmlData.Count == 0) {
                        continue;
                    }
                    foreach (var dr in _htmlData) {
                        try {

                            //<td>吴瑶</td><td>广州凯之源科技有限公司</td><td class="text-center">同意</td><td>广州市荔湾区人力资源和社会保障局</td><td>2023年3月3日 18:43</td><td>2023年3月10日 18:43</td>                     
                            var reg =new Regex("(?<=<td>)(.*?)(?=</td>)", RegexOptions.IgnoreCase);
                            var ms = reg.Matches(dr);

                            fullName = ms[0].Value;
                            unit = ms[1].Value;
                            approvalResult = ms[2].Value;
                            approvalUnit = ms[3].Value;
                            noticeStartTime =Convert.ToDateTime(ms[4].Value);
                            noticeEndTime = Convert.ToDateTime(ms[5].Value);

                            var sql = $"SELECT ID FROM MONITOR_LIST WHERE FULL_NAME='{fullName}' AND UNIT='{unit}'";
                            var isExists = OleDbHelper.Exists(sql);

                            if (!isExists) {

                                var insertSql = $"INSERT INTO MONITOR_LIST(FULL_NAME,UNIT,RESULT,APPROVAL_UNIT,NOTICE_DATE,NOTICE_START_TIME,NOTICE_END_TIME,CREATE_DT) " +
                                    $"VALUES('{fullName}','{unit}','{approvalResult}','{approvalUnit}','{noticeStartTime.ToShortDateString()}','{noticeStartTime}','{noticeEndTime}',NOW())";
                                var effectRows = OleDbHelper.ExecuteSql(insertSql);

                                if (effectRows>0) {
                                    msg = $"{fullName}|{unit}，入库成功！";
                                    //Growl.Info(msg);
                                    LogHelper.Info(msg);
                                }
                                else {
                                    msg = $"{fullName}|{unit}，入库失败！";
                                    Growl.Error(msg);
                                    LogHelper.Error(msg);
                                }
                            }
                        }
                        catch (Exception ex) {
                            msg = $"解析参数或入库失败，{ex?.Message}";

                            Growl.Error(msg);
                            LogHelper.Error(msg);
                        }

                        finally {
                            //刷新界面数据状态
                            System.Windows.Application.Current.Dispatcher.Invoke(() => {
                                this.Query();
                            });

                            this.SetTips(msg);
                        }
                    }
                }
                catch (Exception ex) {
                    msg = $"执行监控JOB失败，{ex?.Message}";

                    Growl.Error(msg);
                    LogHelper.Error(msg);
                }
                finally {
                    Thread.Sleep(jobInterval * 1000);//默认休眠3s
                }
            }
        }

        #endregion
    }

    class DetailModelEqualityComparer : IEqualityComparer<DetailModel> {
        public bool Equals(DetailModel x, DetailModel y) => x.NoticeStartTime.ToString("yyyy/MM/dd") == y.NoticeStartTime.ToString("yyyy/MM/dd") && x.ApprovalUnit == y.ApprovalUnit;

        public int GetHashCode(DetailModel obj) => $"{obj.NoticeStartTime:yyyy/MM/dd}-{obj.ApprovalUnit}".GetHashCode();
    }
}