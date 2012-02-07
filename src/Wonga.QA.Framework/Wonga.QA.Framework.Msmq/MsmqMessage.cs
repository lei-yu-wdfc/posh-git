using System;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Msmq
{
    public abstract class MsmqMessage
    {
        public override string ToString()
        {
            XmlRootAttribute root = GetType().GetAttribute<XmlRootAttribute>();

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<?xml version=\"1.0\" ?>");
            builder.AppendFormat("<Messages xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://tempuri.net/{0}\"", root.Namespace);

            if (!String.IsNullOrEmpty(root.DataType))
            {
                String[] types = root.DataType.Split(',');
                for (Int32 i = 0; i < types.Length; i++)
                    builder.AppendFormat(" xmlns:baseType{0}=\"{1}\"", i == 0 ? null : i.ToString(), types[i]);
            }

            builder.AppendLine(">");
            builder.AppendFormat("<{0}>\n", root.ElementName);

            foreach (PropertyInfo property in GetType().GetProperties())
            {
                String name = property.Name;
                Type type = property.PropertyType;
                Object value = property.GetValue(this, null);

                if (value == null)
                {
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                        builder.AppendFormat("<{0}>{1}</{0}>\n", name, "null");
                }
                else if ((typeof(IList).IsAssignableFrom(type)))
                {
                    builder.AppendFormat("<{0}>\n", name);

                    if (value is Byte[])
                        builder.Append(Data.ToString(value));
                    else
                        foreach (Object o in (IList)value)
                            if (o != null)
                                builder.AppendFormat("<{0}>{1}</{0}>\n", o.GetType().Name, Data.ToString(o));

                    builder.AppendFormat("</{0}>\n", name);
                }
                else
                    builder.AppendFormat("<{0}>{1}</{0}>\n", name, Data.ToString(value));
            }

            builder.AppendFormat("</{0}>\n", root.ElementName);
            builder.AppendLine("</Messages>");

            return builder.ToString();
        }
    }

    public abstract class MsmqMessage<T> : MsmqMessage where T : MsmqMessage<T>
    {

    }
}
