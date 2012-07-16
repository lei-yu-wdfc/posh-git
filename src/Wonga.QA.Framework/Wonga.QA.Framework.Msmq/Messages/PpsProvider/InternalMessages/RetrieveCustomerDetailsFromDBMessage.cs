using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.RetrieveCustomerDetailsFromDBMessage </summary>
    [XmlRoot("RetrieveCustomerDetailsFromDBMessage", Namespace = "Wonga.PpsProvider.InternalMessages", DataType = "")]
    public partial class RetrieveCustomerDetailsFromDBMessage : MsmqMessage<RetrieveCustomerDetailsFromDBMessage>
    {
        public Guid SagaId { get; set; }
    }
}
