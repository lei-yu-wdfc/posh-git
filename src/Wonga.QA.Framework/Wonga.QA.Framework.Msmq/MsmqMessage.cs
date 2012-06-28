using System;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Msmq
{
    public abstract class MsmqMessage:MessageBase
    {

		public override void Default()
		{

		}
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

            string header = null;
            var body = ToString(root.ElementName, this, out header);
            builder.AppendFormat("{2}{1}>\n{0}\n</Messages>", body, header, string.IsNullOrEmpty(header) ? String.Empty : " ");
            return builder.ToString();
        }

        private string ToString(string name, object entity, out string header)
        {
            StringBuilder builder = new StringBuilder().AppendFormat("<{0}>\n", name);
            StringBuilder headerBuilder = new StringBuilder();
            Type type = entity.GetType();
            foreach (PropertyInfo property in type.GetProperties())
            {
                Object value = property.GetValue(entity, null);

                if (value == null)
                {
                    SerializeNull(value, builder, property, headerBuilder);
                }
                else if (value is IList)
                {
                    SerializeList(value, builder, property, headerBuilder);
                }
                else
                    SerializeValue(value, property, builder, headerBuilder);
            }
            header = headerBuilder.ToString();
            return builder.AppendFormat("</{0}>", name).ToString();
        }

        protected virtual void SerializeValue(object value, PropertyInfo property, StringBuilder builder, StringBuilder headerBuilder)
        {
            builder.AppendFormat("<{0}>{1}</{0}>\n", property.Name, Get.ToString(value));
        }

        protected virtual void SerializeList(object value, StringBuilder builder, PropertyInfo property, StringBuilder headerBuilder)
        {
            builder.AppendFormat("<{0}>\n", property.Name);

            if (value is Byte[])
                builder.Append(Get.ToString(value));
            else
                foreach (Object element in value as IList)
                    if (element != null)
                        if (element.GetType().IsPrimitive || element.GetType().IsEnum || element is String || element is Decimal ||
                            element is Guid || element is DateTime || element is TimeSpan || element is DateTimeOffset)
                            builder.AppendFormat("<{0}>{1}</{0}>\n", element.GetType().Name, Get.ToString(element));
                        else
                        {
                            string header;
                            builder.AppendFormat(ToString(element.GetType().Name, element, out header));
                            if(!string.IsNullOrEmpty(header))
                            {
                                headerBuilder.AppendFormat(" {0}", header);
                            }
                        }

            builder.AppendFormat("</{0}>\n", property.Name);
        }

        protected virtual void SerializeNull(object value, StringBuilder builder, PropertyInfo property, StringBuilder headerBuilder)
        {
            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof (Nullable<>))
                builder.AppendFormat("<{0}>null</{0}>\n", property.Name);
        }
    }

    public abstract class MsmqMessage<T> : MsmqMessage where T : MsmqMessage<T>
    {
		public void Initialise()
		{
			Default();
		}

    }
}
