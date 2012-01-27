using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("BusinessProcessScheduledPaymentMessage", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public class BusinessProcessScheduledPaymentCommand : MsmqMessage<BusinessProcessScheduledPaymentCommand>
    {
        public Int32 ApplicationId { get; set; }
        public Guid ApplicationGuid { get; set; }
        public Guid OrganisationId { get; set; }
        public Decimal CollectAmount { get; set; }
    }
}
