using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple.Data;

namespace Wonga.QA.Framework.Data
{
    public class AccountingDatabase : QAFDatabase
    {
        public AccountingDatabase(string connectionString)
            : base(connectionString)
        {

        }
    }
}
