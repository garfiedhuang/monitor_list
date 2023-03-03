using Xp.Resin.Print.Models;
using System.Windows;
using System.Text.RegularExpressions;

namespace Xp.Resin.Print.Views.SystemMgt.Product
{
    /// <summary>
    /// PrintView.xaml 的交互逻辑
    /// </summary>
    public partial class PrintView : Window
    {
        public PrintView(PrintParams model)
        {
            InitializeComponent();

            this.DataContext = new { Model = model };
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtCopies.Text.Trim()))
            {
                this.txtCopies.IsError = true;
                this.txtCopies.ErrorStr = "不能为空";
                return;
            }
            else
            {
                var copies = this.txtCopies.Text.Trim();
                var reg =new Regex ("^([1-9]|[1-9][0-9]|100)$");

                if (!reg.IsMatch(copies))
                {
                    this.txtCopies.IsError = true;
                    this.txtCopies.ErrorStr = "必须是1至100的正整数";
                    return;
                }
            }
            if (string.IsNullOrEmpty(this.dpProductDate.Text))
            {
                this.dpProductDate.IsError = true;
                this.dpProductDate.ErrorStr = "不能为空";
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
