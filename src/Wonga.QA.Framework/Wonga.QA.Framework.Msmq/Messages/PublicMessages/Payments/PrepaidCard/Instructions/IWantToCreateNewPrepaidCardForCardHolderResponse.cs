using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.PrepaidCard.DataEntity;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Payments.PrepaidCard.Instructions
{
    /// <summary> Wonga.PublicMessages.Payments.PrepaidCard.Instructions.IWantToCreateNewPrepaidCardForCardHolderResponse </summary>
    [XmlRoot("IWantToCreateNewPrepaidCardForCardHolderResponse", Namespace = "Wonga.PublicMessages.Payments.PrepaidCard.Instructions", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Payments.PrepaidCard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IWantToCreateNewPrepaidCardForCardHolderResponse : MsmqMessage<IWantToCreateNewPrepaidCardForCardHolderResponse>
    {
        public Guid SagaId { get; set; }
        public ProcessStatusEnum CardStatus { get; set; }
        public String ProviderId { get; set; }
        public String AccountNumber { get; set; }
        public String SerialNumber { get; set; }
        public String CardPan { get; set; }
        public Boolean Successful { get; set; }
        public String OriginalMessageId { get; set; }
    }
}
