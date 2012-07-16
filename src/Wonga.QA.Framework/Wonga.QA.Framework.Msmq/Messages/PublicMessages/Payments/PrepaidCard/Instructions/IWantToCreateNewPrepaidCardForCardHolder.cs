using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.PrepaidCard.DataEntity;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard.Instructions
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.Instructions.IWantToCreateNewPrepaidCardForCardHolder </summary>
    [XmlRoot("IWantToCreateNewPrepaidCardForCardHolder", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard.Instructions", DataType = "")]
    public partial class IWantToCreateNewPrepaidCardForCardHolder : MsmqMessage<IWantToCreateNewPrepaidCardForCardHolder>
    {
        public Guid SagaId { get; set; }
        public Int32 CardId { get; set; }
        public Guid ExternalId { get; set; }
        public Guid CustomerExternalId { get; set; }
        public CardEnum CardType { get; set; }
        public ProcessStatusEnum CardStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public String OriginalMessageId { get; set; }
    }
}
