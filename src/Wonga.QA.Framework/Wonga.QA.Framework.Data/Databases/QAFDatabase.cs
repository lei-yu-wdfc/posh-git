
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simple.Data;

namespace Wonga.QA.Framework.Data
{
    public class QAFDatabase
    {
        public QAFDatabase(string connectionString)
        {
            _db = Database.OpenConnection(connectionString);
        }

        private dynamic _db;
        public dynamic Db
        {
            get { return _db; }
            set { _db = value; }
        }
    }
}
