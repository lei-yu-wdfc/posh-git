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
        public DbDatabase(String connection) : base(connection) { SetLog(); }
        public DbDatabase(String connection, MappingSource mapping) : base(connection, mapping) { SetLog(); }
        public DbDatabase(IDbConnection connection) : base(connection) { SetLog(); }
        public DbDatabase(IDbConnection connection, MappingSource mapping) : base(connection, mapping) { SetLog(); }

        private void SetLog()
        {
            Log = new LogWriter();
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
    }

    public class DbDatabase<T> : DbDatabase where T : DbDatabase<T>
    {
        public DbDatabase(String connection) : base(connection) { }
        public DbDatabase(String connection, MappingSource mapping) : base(connection, mapping) { }
        public DbDatabase(IDbConnection connection) : base(connection) { }
        public DbDatabase(IDbConnection connection, MappingSource mapping) : base(connection, mapping) { }
    }
}
