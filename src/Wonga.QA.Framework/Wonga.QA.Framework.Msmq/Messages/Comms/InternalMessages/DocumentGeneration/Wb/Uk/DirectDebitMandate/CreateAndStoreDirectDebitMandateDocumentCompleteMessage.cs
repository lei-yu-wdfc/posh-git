using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.DocumentGeneration.Wb.Uk.DirectDebitMandate
{
    /// <summary> Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.DirectDebitMandate.CreateAndStoreDirectDebitMandateDocumentCompleteMessage </summary>
    [XmlRoot("CreateAndStoreDirectDebitMandateDocumentCompleteMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.DirectDebitMandate", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreateAndStoreDirectDebitMandateDocumentCompleteMessage : MsmqMessage<CreateAndStoreDirectDebitMandateDocumentCompleteMessage>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FileId { get; set; }
    }
}
