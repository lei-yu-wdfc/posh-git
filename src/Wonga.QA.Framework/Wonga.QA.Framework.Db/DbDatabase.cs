using System;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Diagnostics;
using System.IO;

namespace Wonga.QA.Framework.Db
{
    public class DbDatabase : DataContext
    {
        public DbDatabase(String connection) : base(connection) { Init(); }
        public DbDatabase(String connection, MappingSource mapping) : base(connection, mapping) { Init(); }
        public DbDatabase(IDbConnection connection) : base(connection) { Init(); }
        public DbDatabase(IDbConnection connection, MappingSource mapping) : base(connection, mapping) { Init(); }

        public Box Boxed { get; set; }
        
        private void Init()
        {
            Log = new LogWriter();
            Boxed = new Box(this);
        }

        private class LogWriter : StringWriter
        {
            public override void Write(String value)
            {
                Trace.Write(value);
            }

            public override void Write(Char[] buffer, Int32 index, Int32 count)
            {
                Trace.Write(new String(buffer, index, count));
            }
        }

        public class Box
        {
            private DbDatabase _database;

            public String ConnectionString { get { return _database.Connection.ConnectionString; } }

            public Box(DbDatabase database)
            {
                _database = database;
            }

            public Boolean Exists()
            {
                return _database.DatabaseExists();
            }
        }
    }

    public class DbDatabase<T> : DbDatabase where T : DbDatabase<T>
    {
        public DbDatabase(String connection) : base(connection) { }
        public DbDatabase(String connection, MappingSource mapping) : base(connection, mapping) { }
        public DbDatabase(IDbConnection connection) : base(connection) { }
        public DbDatabase(IDbConnection connection, MappingSource mapping) : base(connection, mapping) { }
    }
}
