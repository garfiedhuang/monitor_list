using System.Windows;
//using EQuality.Tools.Utility;
using System;
using Xp.Resin.Print.Common;
using System.Windows.Documents;

namespace Xp.Resin.Print.Views.SystemMgt.Product
{
    /// <summary>
    /// TestView.xaml 的交互逻辑
    /// </summary>
    public partial class TestView : Window
    {

        private static string _printer;

        public TestView(string printer)
        {
            InitializeComponent();
            _printer = printer;


            #region 初始化打印指令

            txtPrintCommand.Document.Blocks.Clear();

            var innerMessage = string.Empty;

            innerMessage = @"CT~~CD,~CC^~CT~
^XA
~TA000
~JSN
^LT0
^MNW
^MTT
^PON
^PMN
^LH0,0
^JMA
^PR6,6
~SD15
^JUS
^LRN
^CI27
^PA0,1,1,0
^XZ
^XA
{tempName1}
{tempName2}
^MMT
^PW945
^LL591
^LS0
^FO120,49^GFA,429,1840,20,:Z64:eJzt1E1uhCAUAGAIC5YcwfQIvYBeZeYYTVqYo3kUzFzAxkVpwkAfvIeDjiamaXfzVvqF6PtTxp7xd+E2LJQL6csVX5kJJnB/zEz0/2q6fW+Pmn6prCc7owkwWeyTbJS+s/S8b7JQ2Q1NJhv3TVMu8AbfOKxDf2zYG5oCU57sFYekXGXibjKQNWjNlkEdIm5bzk+hdWA8XpZmaxtnY8VS09JssvVo1GXTP1pMZizNKO7bBQwaiDPPxg8alAUGde4ZZIYjyuYfTS0M7tAC5BdDZXC6smbLHJm/W5cNMkLT2PrZfDGFFmrjNhl3PBlML/fqlM1yV1lOkcPJ1lWWF5GMPuomm3DG6dkU2hRjYN3avgKPC5NTnAIrJmNaHDlk8yu71paWSQy3AZpNPyIBSwXR2zMMtCez7Bm/jh9JYowi:090C
^FO194,179^GFA,189,624,16,:Z64:eJxjYKAzkEPj2wDx/x8yqPzfc2Bc+f//GzD4No+J5jM+AGKw/g8Q/vMGCJ8Rwmc+jspnR+Pz///PwGDxGJVf8aAGZj6Y/+PBHxT+B4Q8AzvQvAcKBch8BgcLBhS+/f8fMK79//8PIPwPqHxGJD4DUD+UD9aPxmf8D3QEOh/ZPWh8egIAnCxggw==:6E2B
^FO120,244^GFA,473,936,24,:Z64:eJzV0bFKw0AYB/B/vCFOSUeH4g0+govQQkY3X8IXCHQRlOSkQhwE0UmhxucQKh44OPYFDE0N2K0pOLRD2vO79K4q+AD6ZTl+hMv/+wf4H8O6EmgiBFw6YP/LhXGfDv6L9Zb23gU5VzniwdoTAZYtyYPalbSupJMNyWPyiFsPSu2DKaDIvR8OlUzhzCmQ12TC+GRB3g3hVhQorlyBdn1/mklk1yH8JadvzMfASe2P5M0tev8+0l5av8uko6oQzK8Ar/FmvHOqXekeKNM2cuO7xS35kIwT8fN328/sQaLtmr2CdGncn7Ukog26X+8VvJbG4/JYIqaHa4+rsfW0Q3s9qZVHfetB/0PnUfBDJ8dkrzA97NwUki1Uhc3ajwrTA7siz/qUR/tBWMhVftZ9FqwH69OR9SQRztq9fJQbvwzE6r/rvfzDXBhP+Tf33Nz2cMbxq4uGcT0ek/ib8wmALb4R:2C57
^FO120,310^GFA,457,936,24,:Z64:eJzV0rFKw0AYB/B/vCFb4ygYmsEHcBBESCAv0sFHCHRwsDRXKmQRik4KNe8hrXjg4NgnCA0UdDOBDlnSnl9yubT6BHq3/Ybv/t/3HfA/DhsLwEYAg1/8dN546gFhtm3crXx6S/4u832POFiyDcB+uxRGstSedhv3s8oXOcxz5LDMfYeMyIfkHav1rw35OEDniup0K/fq+nEikNwHsArtg9pfyO0j8k9yJ2v9KRGGLMk/6te090eVS+prUeeXXPnZ6pF8mYO1ruZTPAt4JvU1IS9btwpXYHhA9eUsMHYeZtcCIV2THGXKtMd96utVKu+17s/XVR5yi+r35Lrxk4eVYBtZAqfktiy4mgO7I0/mlEdqH6g9vnE2hcoDOzW1RxE3Wnd2PvF5vXfLr93RHjvK4c5onmmo53DjqH+CY55XufX/4YeNM36JZll/8HwDH0zEug==:30C1
^FO157,375^GFA,317,780,20,:Z64:eJzF0b0KwjAQB/B/rXAOYlaHUn0D1w7FvpLiUlBonqC+gNQXcejm2Ecwm6O6OYgxHzW1WHD0hnD5Ee4uHPDP6Ldu/nakzlhnpbOqw8y7AGBS3msjbZl8/jKWyNIYohCfluqssYkxPV+sGhE3xlS9dTAGSSnAjCXasjDssODTMmN5DMqjlq21FbGo+xpb5RHoMHnbLFNlVlVq69Uza1t22XEBOj2d9QbA8nwF7QJnNARuojb7N0YFcOXCmp2PsUSZp2z/cDZn3LtI2bINlV9294VfaePk+i688sum4LCbbRb8p3gB8WR8Gg==:820B
^FO120,443^GFA,289,936,24,:Z64:eJzV0L0KwjAQB/CEDF2kHXUo1c1H0ELBZ/ENOgoWWnDoY/gqAQdfo48guOhQz1x61ybUj1VvCJcfIfnnhPiTCiqz7GgTr5lVTS4BtIhTqMjPpgGAkdeuly178M7BdNYbhX4ij9ADqTuPU98P5PO78Qw9wfyOm1PFJ8/QS3jw/exF792/TuQzz+27tqaY54Xbf+E4yWHk/bvD/HuH1vP8u/McfJ9sOb/1C3t4Zd8ANJ1jzmTvumzYs1XtuOr9tjxSHuuaPY8iMZTU3C1UKH6znm5Huik=:742E
^FT688,217^BQN,2,7
^FH\^FD  7105100EA3-1B-03^FS
^FPH,6^FO297,187^ASN,40,12^FB91,1,8,L^FH\^FDE38^FS
^FPH,6^FO297,252^XGtempName1,1,1^FS
^FPH,6^FO297,318^ASN,40,12^FB598,1,8,L^FH\^FD7105100EA3-1B-03^FS
^FPH,6^FO297,383^XGtempName2,1,1^FS
^FPH,6^FO297,451^ASN,40,12^FB598,1,8,L^FH\^FD2023-02-14^FS
^PQ1,,,Y
^XZ";

            Run run = new Run(innerMessage);

            Paragraph p = new Paragraph();

            p.Inlines.Add(run);

            txtPrintCommand.Document.Blocks.Add(p);

            #endregion

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(txtPrintCommand.Document.ContentStart, txtPrintCommand.Document.ContentEnd);

            var commands = textRange.Text.Trim();

            if (string.IsNullOrEmpty(commands))
            {
                HandyControl.Controls.Growl.Error("打印指令不能为空！");
                return;
            }

            var font = this.txtFont.Text.Trim();
            if (string.IsNullOrEmpty(font))
            {
                this.txtFont.IsError = true;
                this.txtFont.ErrorStr = "不能为空";
                return;
            }

            var fontHeight = this.txtFontHeight.Text.Trim();
            if (string.IsNullOrEmpty(fontHeight))
            {
                this.txtFontHeight.IsError = true;
                this.txtFontHeight.ErrorStr = "不能为空";
                return;
            }

            var fontWidth = this.txtFontWidth.Text.Trim();
            if (string.IsNullOrEmpty(fontWidth))
            {
                this.txtFontWidth.IsError = true;
                this.txtFontWidth.ErrorStr = "不能为空";
                return;
            }

            var isBold = this.txtIsBold.Text.Trim();
            if (string.IsNullOrEmpty(isBold))
            {
                this.txtIsBold.IsError = true;
                this.txtIsBold.ErrorStr = "不能为空";
                return;
            }

            try
            {

                var temp1 = ZebraPrintHelper.ConvertChineseToHex("前保险杠总成（霜月白）", "tempName1", font, Convert.ToInt32(fontHeight), Convert.ToInt32(fontWidth), Convert.ToInt32(isBold));
                var temp2 = ZebraPrintHelper.ConvertChineseToHex("武汉名杰模塑有限公司", "tempName2", font, Convert.ToInt32(fontHeight), Convert.ToInt32(fontWidth), Convert.ToInt32(isBold));


                commands = commands.Replace("{tempName1}", temp1).Replace("{tempName2}", temp2);


                //RawPrinterHelper.SendStringToPrinter(_printer, commands);

                HandyControl.Controls.Growl.Info("发送成功！");

            }
            catch (Exception ex)
            {
                var _msg = $"发送失败，{ex?.Message}";

                HandyControl.Controls.Growl.Error(_msg);
                LogHelper.Error(_msg);
            }

            return;


            //this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
