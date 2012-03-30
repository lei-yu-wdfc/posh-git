using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple.Data;

namespace Wonga.QA.Framework.Data
{
    public class BiDatabase : QAFDatabase
    {
        public BiDatabase(string connectionString)
            : base(connectionString)
        {
            
        }
    }
}
