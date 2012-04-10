using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Ops;
using Wonga.QA.Framework.Db.Risk;

namespace Wonga.QA.Framework.Db.Extensions
{
	public static partial class DbExtensions
	{
		public static void SetServiceConfiguration(this DbDriver db, string key, string value)
		{
			var serviceConfig = db.GetServiceConfiguration(key);

			if (serviceConfig == null)
			{
				serviceConfig = new ServiceConfigurationEntity { Key = key, Value = value };
				db.Ops.ServiceConfigurations.InsertOnSubmit(serviceConfig);
				db.Ops.SubmitChanges();
			}

			serviceConfig.Value = value;
			serviceConfig.Submit();
		}

		public static void SetServiceConfigurations(this DbDriver db, Dictionary<string, string> keyValuePairs)
		{
			foreach (var keyValuePair in keyValuePairs)
			{
				db.SetServiceConfiguration(keyValuePair.Key, keyValuePair.Value);
			}
		}

		public static ServiceConfigurationEntity GetServiceConfiguration(this DbDriver db, string key)
		{
			return db.Ops.ServiceConfigurations.SingleOrDefault(a => a.Key == key);
		}
	}
}
