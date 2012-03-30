using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple.Data;

namespace Wonga.QA.Framework.Data
{
    public class ColdStorageDatabase : QAFDatabase
    {
        public ColdStorageDatabase(string connectionString)
            : base(connectionString)
        {
            
        }
    }
}
