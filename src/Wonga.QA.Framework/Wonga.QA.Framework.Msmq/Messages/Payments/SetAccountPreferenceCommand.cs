using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("SetAccountPreference", Namespace = "Wonga.Payments", DataType = "")]
    public partial class SetAccountPreferenceCommand : MsmqMessage<SetAccountPreferenceCommand>
    {
        public Guid AccountId { get; set; }
        public Boolean RemindBeforeEndLoan { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
