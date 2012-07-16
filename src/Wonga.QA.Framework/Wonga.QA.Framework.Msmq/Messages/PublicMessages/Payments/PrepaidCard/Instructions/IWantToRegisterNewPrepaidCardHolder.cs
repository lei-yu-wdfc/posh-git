using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.PrepaidCard.DataEntity;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard.Instructions
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.Instructions.IWantToRegisterNewPrepaidCardHolder </summary>
    [XmlRoot("IWantToRegisterNewPrepaidCardHolder", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard.Instructions", DataType = "")]
    public partial class IWantToRegisterNewPrepaidCardHolder : MsmqMessage<IWantToRegisterNewPrepaidCardHolder>
    {
        public Guid SagaId { get; set; }
        public Int32 CardHolderId { get; set; }
        public Guid ExternalId { get; set; }
        public Guid CustomerExternalId { get; set; }
        public String ProviderId { get; set; }
        public ProcessStatusEnum HolderStatus { get; set; }
        public String OriginalMessageId { get; set; }
    }
}
