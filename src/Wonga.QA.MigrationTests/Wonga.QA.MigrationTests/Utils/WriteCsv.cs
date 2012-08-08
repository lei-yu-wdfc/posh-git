
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace Wonga.QA.MigrationTests
{
    class WriteCsv : StreamWriter
    {
        public WriteCsv(Stream stream): base(stream)
        {
        }

        public WriteCsv(string filename): base(filename)
        {
        }

        public void WriteRow(CsvRow row)
        {
            StringBuilder builder = new StringBuilder();
            bool firstColumn = true;
            foreach (string value in row)
            {
                // Add separator if this isn't the first value
                if (!firstColumn)
                    builder.Append(',');
                // Implement special handling for values that contain comma or quote
                // Enclose in quotes and double up any double quotes
                if (value.IndexOfAny(new char[] { '"', ',' }) != -1)
                    builder.AppendFormat("\"{0}\"", value.Replace("\"", "\"\""));
                else
                    builder.Append(value);
                firstColumn = false;
            }
            row.LineText = builder.ToString();
            WriteLine(row.LineText);
        }
    }

    public class CsvRow : List<string>
    {
        public string LineText { get; set; }
    }
}
