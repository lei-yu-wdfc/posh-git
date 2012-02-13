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

            StringBuilder builder = new StringBuilder().AppendFormat("<?xml version=\"1.0\" ?>\n<Messages xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://tempuri.net/{0}\"", root.Namespace);

            if (!String.IsNullOrEmpty(root.DataType))
            {
                String[] types = root.DataType.Split(',');
                for (Int32 i = 0; i < types.Length; i++)
                    builder.AppendFormat(" xmlns:baseType{0}=\"{1}\"", i == 0 ? null : i.ToString(), types[i]);
            }

            return builder.AppendFormat(">\n{0}\n</Messages>", ToString(root.ElementName, this)).ToString();
        }

        private String ToString(String name, Object entity)
        {
            StringBuilder builder = new StringBuilder().AppendFormat("<{0}>\n", name);

            foreach (PropertyInfo property in entity.GetType().GetProperties())
            {
                Object value = property.GetValue(entity, null);

                if (value == null)
                {
                    if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        builder.AppendFormat("<{0}>null</{0}>\n", property.Name);
                }
                else if (value is IList)
                {
                    builder.AppendFormat("<{0}>\n", property.Name);

                    if (value is Byte[])
                        builder.Append(Data.ToString(value));
                    else
                        foreach (Object element in value as IList)
                            if (element != null)
                                if (element.GetType().IsPrimitive || element.GetType().IsEnum || element is String || element is Decimal || element is Guid || element is DateTime || element is TimeSpan || element is DateTimeOffset)
                                    builder.AppendFormat("<{0}>{1}</{0}>\n", element.GetType().Name, Data.ToString(element));
                                else
                                    builder.AppendFormat(ToString(element.GetType().Name, element));

                    builder.AppendFormat("</{0}>\n", property.Name);
                }
                else
                    builder.AppendFormat("<{0}>{1}</{0}>\n", property.Name, Data.ToString(value));
            }

            return builder.AppendFormat("</{0}>", name).ToString();
        }
    }

    public abstract class MsmqMessage<T> : MsmqMessage where T : MsmqMessage<T> { }
}
