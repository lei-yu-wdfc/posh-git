using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IBusinessPaymentCardAdded </summary>
    [XmlRoot("IBusinessPaymentCardAdded", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IBasePaymentCardAdded,Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IBusinessPaymentCardAddedEvent : MsmqMessage<IBusinessPaymentCardAddedEvent>
    {
        public Guid OrganisationId { get; set; }
        public Guid PaymentCardId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
