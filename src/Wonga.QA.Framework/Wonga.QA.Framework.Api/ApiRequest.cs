using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public abstract class ApiRequest:MessageBase
    {
        public override String ToString()
        {
            String root = GetType().GetAttribute<XmlRootAttribute>().ElementName;
            PropertyInfo[] properties = GetType().GetProperties();

            if (!properties.Any())
                return String.Format("<{0} />", root);

            StringBuilder builder = new StringBuilder().AppendFormat("<{0}>", root);
            foreach (PropertyInfo property in properties)
            {
                Object value = property.GetValue(this, null);
                if (value == null)
                    builder.AppendFormat("<{0} xsi:nil=\"true\" />", property.Name);
                else
                {
                    String convert = Get.ToString(value);
                    if (String.IsNullOrEmpty(convert))
                        builder.AppendFormat("<{0} />", property.Name);
                    else
                        builder.AppendFormat("<{0}>{1}</{0}>", property.Name, convert);
                }
            }
            return builder.AppendFormat("</{0}>", root).ToString();
        }
    }



	public abstract class ApiRequest<T> : ApiRequest where T : ApiRequest<T>, new()
    {
        public static T New(Action<T> action = null)
        {
            T t = new T();
            t.Default();
            if (action != null)
                action(t);
            return t;
        }
    }
}
