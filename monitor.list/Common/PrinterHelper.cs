using System;
using System.Collections.Generic;
using System.Drawing.Printing;

namespace Xp.Resin.Print.Common
{
    public class PrinterHelper
    {
        private static PrintDocument fPrintDocument = new PrintDocument();
        //获取本机默认打印机名称
        public static String DefaultPrinter()
        {
            return fPrintDocument.PrinterSettings.PrinterName;
        }
        public static List<String> GetLocalPrinters()
        {
            List<String> fPrinters = new List<String>();
            fPrinters.Add(DefaultPrinter()); //默认打印机始终出现在列表的第一项
            foreach (String fPrinterName in PrinterSettings.InstalledPrinters)
            {
                if (!fPrinters.Contains(fPrinterName))
                {
                    fPrinters.Add(fPrinterName);
                }
            }
            return fPrinters;
        }

        public static Dictionary<string, bool> GetPrinters()
        {
            var result = new Dictionary<string, bool>();

            var defaultPrinter = DefaultPrinter();
            var localPrinters = GetLocalPrinters();

            if (localPrinters != null)
            {
                localPrinters.ForEach(printer =>
                {
                    result.Add(printer, defaultPrinter == printer);
                });
            }

            return result;
        }
    }
}
