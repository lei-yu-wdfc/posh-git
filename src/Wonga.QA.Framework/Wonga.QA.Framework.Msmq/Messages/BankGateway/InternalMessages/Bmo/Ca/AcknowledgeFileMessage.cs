using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Bmo.Ca
{
    /// <summary> Wonga.BankGateway.InternalMessages.Bmo.Ca.AcknowledgeFileMessage </summary>
    [XmlRoot("AcknowledgeFileMessage", Namespace = "Wonga.BankGateway.InternalMessages.Bmo.Ca", DataType = "" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages.Bmo.Ca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class AcknowledgeFileMessage : MsmqMessage<AcknowledgeFileMessage>
    {
        public Int32 FileSequenceNumber { get; set; }
        public Boolean WasAccepted { get; set; }
        public String InfoMessage { get; set; }
        public String FileName { get; set; }
        public Byte[] FileContents { get; set; }
    }
}
