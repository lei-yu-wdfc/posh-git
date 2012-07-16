using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IApplicationWrittenOff </summary>
    [XmlRoot("IApplicationWrittenOff", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IApplicationWrittenOff : MsmqMessage<IApplicationWrittenOff>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Decimal Balance { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
