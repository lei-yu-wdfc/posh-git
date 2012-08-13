using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Scotiabank.Ca
{
    /// <summary> Wonga.BankGateway.InternalMessages.Scotiabank.Ca.SendScotiaAcknowledgeFileMessage </summary>
    [XmlRoot("SendScotiaAcknowledgeFileMessage", Namespace = "Wonga.BankGateway.InternalMessages.Scotiabank.Ca", DataType = "Wonga.BankGateway.Core.Interfaces.IAcknowledgeFileMessage" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages.Scotiabank.Ca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SendScotiaAcknowledgeFileMessage : MsmqMessage<SendScotiaAcknowledgeFileMessage>
    {
        public String FileName { get; set; }
        public Byte[] FileContents { get; set; }
        public Int32 AcknowledgeTypeId { get; set; }
    }
}
