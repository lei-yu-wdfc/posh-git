using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using Wonga.QA.Tools.ReportParser;

namespace Wonga.QA.Tools.ReportConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            string source = args[0];
            string destination = args[1];
            string format = args[2];
            string output = "";

            TestReport testReport = new GallioReportParser(XDocument.Load(source)).GetTestReport();

            if (format.ToLower() == "html")
                output = new HtmlFormatTestReport().FormatReport(testReport);
            else if (format.ToLower() == "csv")
                output = new CsvFormatTestReport().FormatReport(testReport);

            File.WriteAllText(destination, output);
        }

        public static string GetCurrentFolder()
        {
            //get the full location of the assembly with DaoTests in it
            string fullPath = System.Reflection.Assembly.GetAssembly(typeof(Program)).Location;

            //get the folder that's in
            return Path.GetDirectoryName(fullPath);
        }
    }
}
