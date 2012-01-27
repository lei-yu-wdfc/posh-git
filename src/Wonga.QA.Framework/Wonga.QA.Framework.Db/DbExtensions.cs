using System.Data.Linq;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Db
{
    public static class DbExtensions
    {
        public static Table<T> SetTable<T>(this Table<T> table) where T : DbEntity<T>
        {
            table.ForEach(t => t.Table = table);
            return table;
        }
    }
}
