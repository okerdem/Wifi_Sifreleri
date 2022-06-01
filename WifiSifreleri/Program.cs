using System;
using System.IO;
using System.Xml;

namespace WifiSifreleri
{
    class Program
    {
        static void Main(string[] args)
        {
            string directory = @"C:\wifisifreleri";
            string sifreler = @"C:\wifisifreleritxt";
            //Bilgilerin kaydedileceği dosyanın oluşturulması.
            Directory.CreateDirectory(directory);
            Directory.CreateDirectory(sifreler);
            string cmdText = "/C netsh wlan export profile folder="+directory+" key=clear";
            //CMD ile wifi bilgilerinin ,oluşturulan dosyaya kaydedilmesi.
            System.Diagnostics.Process.Start("CMD.exe", cmdText);

            //Wifi bilgilerini içeren dosyaların adlarının alınması.
            string[] dosyaAdlari = Directory.GetFiles(directory);
            string bilgiler="";
            //Adı alınan dosyaların içinden istenen verilerin alınması.
            foreach (string filename in dosyaAdlari)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);
                XmlNodeList nList = doc.GetElementsByTagName("keyMaterial");
                foreach(XmlNode node in nList) 
                {
                    string sifre = node.InnerText;
                    bilgiler += filename + " " + sifre + "\n";
                }
                nList = null;
                doc = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            File.WriteAllText(@"C:\wifisifreleritxt\sifreler.txt", bilgiler);
            Console.Write(bilgiler);
            Console.ReadKey();
        }
    }
}
