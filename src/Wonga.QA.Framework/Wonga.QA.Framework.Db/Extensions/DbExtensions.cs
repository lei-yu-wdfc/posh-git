using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Db.Risk;

namespace Wonga.QA.Framework.Db
{
    public static partial class DbExtensions
    {
        public static T Insert<T>(this Table<T> table, T entity) where T : DbEntity<T>
        {
            table.InsertOnSubmit(entity);
            return entity;
        }

		public static String GetName<T>(this Table<T> table) where T : DbEntity<T>
		{
			return table.Context.Mapping.GetTable(typeof (T)).TableName;
		}

    	public static void AddSurnameToBlacklist(this DbDriver db, String surname)
        {
            var blacklistEntity = new Blacklist.BlackListEntity()
                                      {
                                          LastName = surname,
                                          ExternalId = Guid.NewGuid(),
                                      };
            db.Blacklist.BlackLists.Insert(blacklistEntity);
            db.Blacklist.SubmitChanges();
        }
    }
}
