using System.Runtime.InteropServices;
using System.Text;

namespace Xp.Resin.Print.Common
{
    public class ZebraPrintHelper
    {
        [DllImport("Fnthex32.dll")]
        public static extern int GETFONTHEX(
        string BarcodeText,//转换的文本
        string FontName,//打印字体
        string FileName,//存储的变量名称
        int Orient,//方向
        int Height,//字体高度,点阵高度
        int Width,//点阵宽度
        int IsBold,//是否加粗0,1
        int IsItalic,//是否斜体0,1
        StringBuilder ReturnBarcodeCMD);//存储的内容

        /// <summary>
        /// 转换中文
        /// </summary>
        /// <param name="chStr">转换的字符</param>
        /// <param name="tempName">存储的变量名称</param>
        /// <param name="font">使用的字体</param>
        /// <returns></returns>
        public static string ConvertChineseToHex(string chStr, string tempName, string font = "Microsoft YaHei",int fontHeight=40,int fontWidth=20, int isBold=1)
        {
            StringBuilder cBuf = new StringBuilder(chStr.Length * 1024);
            //int nCount = GETFONTHEX(chStr, font, tempName, 0, 25, 15, 1, 0, cBuf);

            int nCount = GETFONTHEX(chStr, font, tempName, 0, fontHeight, fontWidth, isBold, 0, cBuf);

            string temp = " " + cBuf.ToString();
            temp = temp.Substring(0, nCount);
            return temp;
        }
    }
}
