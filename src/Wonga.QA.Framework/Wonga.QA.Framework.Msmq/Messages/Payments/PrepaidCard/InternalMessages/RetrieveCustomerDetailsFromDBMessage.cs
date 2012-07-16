using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PrepaidCard.InternalMessages
{
    /// <summary> Wonga.Payments.PrepaidCard.InternalMessages.RetrieveCustomerDetailsFromDBMessage </summary>
    [XmlRoot("RetrieveCustomerDetailsFromDBMessage", Namespace = "Wonga.Payments.PrepaidCard.InternalMessages", DataType = "")]
    public partial class RetrieveCustomerDetailsFromDBMessage : MsmqMessage<RetrieveCustomerDetailsFromDBMessage>
    {
        public Guid SagaId { get; set; }
    }
}
