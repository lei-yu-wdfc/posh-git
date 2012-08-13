using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Wonga.QA.Framework.UI.UiElements.Pages;

namespace Wonga.QA.Framework.UI.Ui.Pages.Helpers
{
    public class PropertiesHelper<T> where T : BasePage
    {
        private static List<PropertyInfo> GetPropertyList()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            return properties.ToList();
        }

        private static PropertyInfo GetPropertyInfo(String fieldName)
        {
            PropertyInfo property = typeof(T).GetProperty(fieldName);
            return property;
        }

        public static void SetFieldValue(T obj, String fieldName, String value)
        {
            PropertyInfo property = GetPropertyInfo(fieldName);
            if (property == null)
                throw new Exception(String.Format("Incorrect field name: '{0}' for {1} page", fieldName, obj.GetType()));
            property.SetValue(obj, value, null);
        }

        public static Object GetFieldValue(T obj, String fieldName)
        {
            PropertyInfo property = GetPropertyInfo(fieldName);
            if (property == null)
                throw new Exception(String.Format("Incorrect field name: '{0}' for {1} page", fieldName, obj.GetType()));
            Object value = property.GetValue(obj, null);
            return value;
        }
    }
}
