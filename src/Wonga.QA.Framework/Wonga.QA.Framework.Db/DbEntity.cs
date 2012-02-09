using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Wonga.QA.Framework.Db
{
    public abstract class DbEntity
    {
        private DbDatabase _database;
        public DbDatabase Database { get { return _database ?? (_database = (DbDatabase)this.GetContext()); } }
    }

    public static class DbEntityEx
    {
        public static DataContext GetContext(this DbEntity entity)
        {
            return entity.GetField<PropertyChangingEventHandler>("PropertyChanging").GetInvocationList().First().Target.GetField<DataContext>("services", "context");
        }

        private static T GetField<T>(this Object value, params String[] names)
        {
            return (T)names.Aggregate(value, (v, n) => v.GetType().GetField(n, BindingFlags.Instance | BindingFlags.NonPublic).GetValue(v));
        }
    }

    public abstract class DbEntity<T> : DbEntity where T : DbEntity<T>
    {
        public T Refresh()
        {
            Database.Refresh(RefreshMode.OverwriteCurrentValues, this);
            return (T)this;
        }

        public void Submit(Boolean force = false)
        {
            try
            {
                Database.SubmitChanges();
            }
            catch (ChangeConflictException)
            {
                if (force)
                {
                    Database.ChangeConflicts.ResolveAll(RefreshMode.KeepCurrentValues);
                    Database.SubmitChanges();
                }
                else throw;
            }
        }
    }
}
