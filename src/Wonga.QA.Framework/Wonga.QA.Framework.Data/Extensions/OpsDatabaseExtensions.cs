using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.Data.Extensions
{
	public static class OpsDatabaseExtensions
	{
		/// <summary>
		/// sets the configuration with a new value for the given key and returns the current value
		/// </summary>
		/// <typeparam name="T">generic type template</typeparam>
		/// <param name="opsDatabase">the database that contains the service configurations table</param>
		/// <param name="keyName">the name of the key to update</param>
		/// <param name="value">the new value for the key</param>
		/// <returns>the current value</returns>
		public static T SetServiceConfiguration<T>(this OpsDatabase opsDatabase, string keyName, T value)
		{
			var row = opsDatabase.Db.ServiceConfigurations.FindByKey(keyName);
			var oldValue = ConvertToType<T>(row.Value);
			opsDatabase.Db.ServiceConfigurations.UpdateByKey(Key: keyName, Value: value.ToString());
			return oldValue;
		}

		private static T ConvertToType<T>(string value)
		{
			Type targetType = typeof(T);

			if (targetType.IsEnum)
			{
				return (T)Enum.Parse(targetType, value);
			}

			if (targetType == typeof(Guid))
			{
				Guid uniqueValue = Guid.Parse(value);
				return (T)Convert.ChangeType(uniqueValue, targetType, CultureInfo.InvariantCulture);
			}

			return (T)Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
		}
	}
}
