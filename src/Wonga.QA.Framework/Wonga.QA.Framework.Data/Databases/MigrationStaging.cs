using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.Data
{
    public class MigrationStaging : QAFDatabase
    {
        public MigrationStaging(string connectionString)
            : base(connectionString)
        {

        }
    }
}
