using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.Ops;

namespace Wonga.QA.Tests.Risk
{
	public static class ServiceConfigurationExtensions
	{
		/// <summary>
		/// sets the configuration with a new value for the given key
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="serviceConfigurations">the service configurations table</param>
		/// <param name="keyName">the name of the key to update</param>
		/// <param name="value">the new value for the key</param>
		/// <returns>the current value</returns>
		public static T SetServiceConfiguration<T>(this System.Data.Linq.Table<ServiceConfigurationEntity> serviceConfigurations, string keyName, T value)
		{
			var row = serviceConfigurations.Single(v => v.Key == keyName);
			var oldValue = (T)Convert.ChangeType(row.Value, typeof(T));
			row.Value = value.ToString();
			row.Submit();
			return oldValue;
		}		
	}
}
