using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple.Data;

namespace Wonga.QA.Framework.Data
{
    public class OpsDatabase : QAFDatabase
    {
        public OpsDatabase(string connectionString)
            : base(connectionString)
        {
            
        }
    }
}
