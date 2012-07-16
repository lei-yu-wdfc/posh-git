using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard.Instructions
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.Instructions.IWantToGiveCashBackResponse </summary>
    [XmlRoot("IWantToGiveCashBackResponse", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard.Instructions", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IWantToGiveCashBackResponse : MsmqMessage<IWantToGiveCashBackResponse>
    {
        public Boolean Successful { get; set; }
        public Guid SagaId { get; set; }
    }
}
