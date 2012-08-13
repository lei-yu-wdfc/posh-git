using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages
{
    /// <summary> Wonga.Comms.InternalMessages.CreatePreTopUpDocumentMessage </summary>
    [XmlRoot("CreatePreTopUpDocumentMessage", Namespace = "Wonga.Comms.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreatePreTopUpDocumentMessage : MsmqMessage<CreatePreTopUpDocumentMessage>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid TopupId { get; set; }
    }
}
