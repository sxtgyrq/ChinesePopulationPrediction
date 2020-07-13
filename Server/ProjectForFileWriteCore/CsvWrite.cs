using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProjectForFileWriteCore
{
    public class CsvWrite
    {
        public static void Write(DateTime recordT, string addr, decimal moneyCount, string md5_plainText, string md5_secretText)
        {
            var m = new string[] { recordT.ToString("yyyy-MM-dd HH:mm:ss"), addr, moneyCount.ToString("f2"), md5_plainText, md5_secretText };
            writeItem(m);
        }

        static void writeItem(string[] input)
        {
            var itemStre = "";
            for (int i = 0; i < input.Length; i++)
            {
                var item = input[i].Replace(",", "__");
                itemStre += item.Trim();
                itemStre += ",";

            }
            if (itemStre.Length > 0)
            {
                itemStre = itemStre.Substring(0, itemStre.Length - 1);
                itemStre += Environment.NewLine;
                var fileName = $"{""}M{DateTime.Now.ToString("yyyyMM")}.csv";
                File.AppendAllText(fileName, itemStre);
            }

        }
        static string path = "C:\\DB\\moneyinput\\";

    }
}
