using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.PrepaidCard.DataEntity;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard.Instructions
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.Instructions.IWantToRegisterNewPrepaidCardHolderResponse </summary>
    [XmlRoot("IWantToRegisterNewPrepaidCardHolderResponse", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard.Instructions", DataType = "")]
    public partial class IWantToRegisterNewPrepaidCardHolderResponseEvent : MsmqMessage<IWantToRegisterNewPrepaidCardHolderResponseEvent>
    {
        public Guid SagaId { get; set; }
        public ProcessStatusEnum HolderStatus { get; set; }
        public Boolean Successful { get; set; }
        public String OriginalMessageId { get; set; }
    }
}
