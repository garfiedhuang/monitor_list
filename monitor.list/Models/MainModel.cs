using GalaSoft.MvvmLight;
using System;
using System.Windows;

namespace Monitor.List.Models
{
    /// <summary>
    /// MainView模型
    /// </summary>
    public class BaseModel : ObservableObject
    {
        /// <summary>
        /// 是否采集模式？
        /// </summary>
        private bool isMaintenanceMode = false;
        public bool IsMaintenanceMode
        {
            get { return isMaintenanceMode; }
            set
            {
                if (value)
                {
                    MaintenanceBtnContent = "停止采集";
                    VisibilityModel = Visibility.Visible;
                }
                else
                {
                    MaintenanceBtnContent = "开始采集";
                    VisibilityModel = Visibility.Hidden;
                }

                isMaintenanceMode = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 是否采集按钮显示文本
        /// </summary>
        private string maintenanceBtnContent = "开始采集";
        public string MaintenanceBtnContent
        {
            get { return maintenanceBtnContent; }
            set
            {
                maintenanceBtnContent = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 元素可见性
        /// </summary>
        private Visibility visibilityModel = Visibility.Hidden;
        public Visibility VisibilityModel
        {
            get { return visibilityModel; }
            set
            {
                visibilityModel = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 监控任务数量
        /// </summary>
        private int monitorTaskCount;
        public int MonitorTaskCount
        {
            get { return monitorTaskCount; }
            set { monitorTaskCount = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 数据入库提示
        /// </summary>
        private string dataTips = "数据采集中，请等待处理...";
        public string DataTips {
            get { return dataTips; }
            set { dataTips = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 汇总提示
        /// </summary>
        private string summaryTips;
        public string SummaryTips {
            get { return summaryTips; }
            set { summaryTips = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 姓名
        /// </summary>
        private string name = "";
        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 单位名称
        /// </summary>
        private string unit = "";
        public string Unit
        {
            get { return unit; }
            set { unit = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 审批状态
        /// </summary>
        private string result = "";
        public string Result
        {
            get { return result; }
            set { result = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 审批单位
        /// </summary>
        private string approvalUnit = "";
        public string ApprovalUnit
        {
            get { return approvalUnit; }
            set { approvalUnit = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 公示日期
        /// </summary>
        private string noticeDate = "";
        public string NoticeDate
        {
            get { return noticeDate; }
            set { noticeDate = value; RaisePropertyChanged(); }
        }
    }

    public class PageModel: ObservableObject
    {
        /// <summary>
        /// 最大页面数
        /// </summary>
        private int maxPageCount = 1;

        public int MaxPageCount
        {
            get { return maxPageCount; }
            set
            {
                maxPageCount = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 当前页数
        /// </summary>
        private int pageIndex = 1;

        public int PageIndex
        {
            get { return pageIndex; }
            set
            {
                pageIndex = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 分页大小
        /// </summary>
        private int dataCountPerPage = 20;

        public int DataCountPerPage
        {
            get { return dataCountPerPage; }
            set
            {
                dataCountPerPage = value;
                RaisePropertyChanged();
            }
        }
    }

    public class DetailModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public long RowNum { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 批复结果
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 审批单位
        /// </summary>
        public string ApprovalUnit { get; set; }

        /// <summary>
        /// 告示开始时间
        /// </summary>
        public DateTime NoticeStartTime { get; set; }


        /// <summary>
        /// 告示结束时间
        /// </summary>
        public DateTime NoticeEndTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDt { get; set; }


        /// <summary>
        /// 审批日期
        /// </summary>
        public DateTime NoticeDt { get; set; }

    }

    public class SummaryModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 行号
        /// </summary>
        public long RowNum { get; set; }

        /// <summary>
        /// 公示日期
        /// </summary>
        public string NoticeDate { get; set; }

        /// <summary>
        /// 审批单位
        /// </summary>
        public string ApprovalUnit { get; set; }

        /// <summary>
        /// 审批数量
        /// </summary>
        public int Summary { get; set; }

    }

    /// <summary>
    /// 打印参数
    /// </summary>
    public class PrintParams
    {
        /// <summary>
        /// 打印机
        /// </summary>
        public string Printer { get; set; }

        /// <summary>
        /// 打印份数
        /// </summary>
        public int Copies { get; set; } = 1;

        public string CarType { get; set; }
        public string MaterialNo { get; set; }
        public string MaterialName { get; set; }
        public string SupplierName { get; set; }
        public DateTime ProductDate { get; set; } = DateTime.Now;
    }

    public class ConfigModel
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 键
        /// </summary>
        public string ConfigKey { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string ConfigValue { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
