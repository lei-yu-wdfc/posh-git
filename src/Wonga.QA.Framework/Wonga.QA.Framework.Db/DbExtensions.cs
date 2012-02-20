using System;
using System.Data.Linq;

namespace Wonga.QA.Framework.Db
{
    public static class DbExtensions
    {
        public static T Insert<T>(this Table<T> table, T entity) where T : DbEntity<T>
        {
            table.InsertOnSubmit(entity);
            return entity;
        }

        public static String GetName<T>(this Table<T> table) where T : DbEntity<T>
        {
            return table.Context.Mapping.GetTable(typeof(T)).TableName;
        }
    }
}
