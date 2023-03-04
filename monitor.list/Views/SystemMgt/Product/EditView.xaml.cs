using Monitor.List.Common;
using Monitor.List.Models;
using System;
using System.Windows;

namespace Monitor.List.Views.SystemMgt.Product
{
    /// <summary>
    /// EditView.xaml 的交互逻辑
    /// </summary>
    public partial class EditView : Window
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        private readonly long _id;

        public EditView(DetailModel model)
        {
            InitializeComponent();

            _id = model.Id;

            this.DataContext = new { Model = model };
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var carType = this.txtCarType.Text.Trim();

            if (string.IsNullOrEmpty(carType))
            {
                this.txtCarType.IsError = true;
                this.txtCarType.ErrorStr = "不能为空";
                return;
            }
            else if(carType.Length>10)
            {
                this.txtCarType.IsError = true;
                this.txtCarType.ErrorStr = "不能超过10个英文字符";
                return;
            }

            var materialNo = this.txtMaterialNo.Text.Trim();

            if (string.IsNullOrEmpty(materialNo))
            {
                this.txtMaterialNo.IsError = true;
                this.txtMaterialNo.ErrorStr = "不能为空";
                return;
            }
            else if (materialNo.Length>24)
            {
                this.txtMaterialNo.IsError = true;
                this.txtMaterialNo.ErrorStr = "不能超过24个英文字符";
            }

            var materialName = this.txtMaterialName.Text.Trim();

            if (string.IsNullOrEmpty(materialName))
            {
                this.txtMaterialName.IsError = true;
                this.txtMaterialName.ErrorStr = "不能为空";
                return;
            }
            else if (materialName.Length > 30)
            {
                this.txtMaterialName.IsError = true;
                this.txtMaterialName.ErrorStr = "不能超过15个中文字符";
                return;
            }

            var supplierName = this.txtSupplierName.Text.Trim();
            if (string.IsNullOrEmpty(supplierName))
            {
                this.txtSupplierName.IsError = true;
                this.txtSupplierName.ErrorStr = "不能为空";
                return;
            }
            else if (supplierName.Length > 30)
            {
                this.txtSupplierName.IsError = true;
                this.txtSupplierName.ErrorStr = "不能超过15个中文字符";
                return;
            }

            var sql = "";
            int rowCount = 0;

            if (_id == 0)
            {//新增
                sql = $"SELECT COUNT(1) FROM BASE_PRODUCT WHERE CAR_TYPE='{this.txtCarType.Text.Trim()}' AND MATERIAL_NO='{this.txtMaterialNo.Text.Trim()}' AND IS_DELETED=0";
            }
            else
            { //修改
                sql = $"SELECT COUNT(1) FROM BASE_PRODUCT WHERE CAR_TYPE='{this.txtCarType.Text.Trim()}' AND MATERIAL_NO='{this.txtMaterialNo.Text.Trim()}' AND IS_DELETED=0 AND ID<>{_id}";
            }
            rowCount = Convert.ToInt32(OleDbHelper.GetSingle(sql) ?? "0");

            if (rowCount > 0)
            {
                HandyControl.Controls.Growl.Warning($"数据库中已存在【{txtCarType.Text}|{txtMaterialNo.Text}】记录");
                return;
            }

            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
