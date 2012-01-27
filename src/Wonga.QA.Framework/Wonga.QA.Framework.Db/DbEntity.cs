using System.Data.Linq;

namespace Wonga.QA.Framework.Db
{
    public abstract class DbEntity
    {
    }

    public abstract class DbEntity<T> : DbEntity where T : DbEntity<T>
    {
        public Table<T> Table { get; set; }

        public T Refresh()
        {
            Table.Context.Refresh(RefreshMode.OverwriteCurrentValues, this);
            return (T)this;
        }
    }
}
