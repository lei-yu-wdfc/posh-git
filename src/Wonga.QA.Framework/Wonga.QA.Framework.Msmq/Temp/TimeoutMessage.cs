using System;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Msmq
{
    [XmlRoot("TimeoutMessage", Namespace = "NServiceBus.Saga", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class TimeoutMessage : MsmqMessage<TimeoutMessage>
    {
        public DateTime Expires { get; set; }
        public Guid SagaId { get; set; }
        public Object State { get; set; }
        public Boolean ClearTimeout { get; set; }

        protected override void SerializeValue(object value, PropertyInfo property, StringBuilder builder, StringBuilder headerBuilder)
        {
            if (property.Name.Equals("State", StringComparison.InvariantCultureIgnoreCase))
            {
                string typeName = value.GetType().Name;
                headerBuilder.AppendFormat("xmlns:{0}=\"{1}\"", typeName.ToLower(), typeName);
                builder.AppendFormat("<{2}:{0}>{1}</{2}:{0}>\n", property.Name, Get.ToString(value), typeName.ToLower());
            }
            else
            {
                base.SerializeValue(value, property, builder, headerBuilder);
            }
        }
    }
}
