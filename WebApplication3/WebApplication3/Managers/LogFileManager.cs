using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace WebApplication3.Managers
{
    public class LogFileManager
    {
        public void writeToLogFile(string eventKey, string email, List<string> skus)
        {
            string path = ConfigurationManager.AppSettings["LogDirectory"];

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(path, $"dailylog_{DateTime.Now.ToString("yyyyMMdd")}.csv");

            if (System.IO.File.Exists(path))
            {
                using (var sw = File.AppendText(path))
                {
                    writeDetails(sw, eventKey, email, skus);
                }
            }
            else
            {
                using (var sw = File.CreateText(path))
                {
                    writeDetails(sw, eventKey, email, skus);
                }
            }
        }

        public void writeDetails(StreamWriter sw, string eventKey, string email, List<string> skus)
        {
            sw.WriteLine(DateTime.Now);
            sw.WriteLine(eventKey);
            sw.WriteLine(email);

            foreach (string sku in skus)
            {
                sw.WriteLine(sku);
            }

            sw.WriteLine();
        }
    }
}