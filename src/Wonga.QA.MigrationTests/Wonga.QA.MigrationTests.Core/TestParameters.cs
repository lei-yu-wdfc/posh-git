using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.MigrationTests.Core
{
    public class TestParameters
    {
        public string Exception { get; set; }
        public string ParametersUsed { get; set; }

        public override string ToString()
        {
            var str = new StringBuilder();
            str.Append("Exception: ");
            str.Append(Exception);
            str.Append("Parameters Used: ");
            str.Append(ParametersUsed);
            return str.ToString();
        }
    }
}
