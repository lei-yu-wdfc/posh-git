using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Wonga.QA.Tools.Tests
{
    public static class GallioReportsRepository
    {
        public static string GetPathForFullGallioReport()
        {
            return Path.Combine(Environment.CurrentDirectory, "FullGallioReport.xml");
        }

        public static string GetPathForEmptyGallioReport()
        {
            return Path.Combine(Environment.CurrentDirectory, "EmptyGallioReport.xml");
        }
    }
}
