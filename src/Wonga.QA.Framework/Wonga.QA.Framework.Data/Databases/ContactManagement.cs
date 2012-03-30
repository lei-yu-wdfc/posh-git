using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple.Data;

namespace Wonga.QA.Framework.Data
{
    public class ContactManagementDatabase : QAFDatabase
    {
        public ContactManagementDatabase(string connectionString)
            : base(connectionString)
        {
            
        }
    }
}
