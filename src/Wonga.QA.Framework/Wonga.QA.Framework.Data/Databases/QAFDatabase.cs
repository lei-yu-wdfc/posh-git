
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Simple.Data;

namespace Wonga.QA.Framework.Data
{
    public class QAFDatabase
    {
        public string ConnectionString { get; private set; }

        public QAFDatabase(string connectionString)
        {
            ConnectionString = connectionString;
            _db = Database.OpenConnection(connectionString);
        }

        private dynamic _db;
        public dynamic Db
        {
            get { return _db; }
            set { _db = value; }
        }

        public string Server
        {
            get { return new SqlConnectionStringBuilder(ConnectionString).DataSource; }
        }
    }
}
