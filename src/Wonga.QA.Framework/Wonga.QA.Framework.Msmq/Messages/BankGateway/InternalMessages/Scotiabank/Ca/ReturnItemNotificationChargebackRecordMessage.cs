using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Scotiabank.Ca
{
    /// <summary> Wonga.BankGateway.InternalMessages.Scotiabank.Ca.ReturnItemNotificationChargebackRecordMessage </summary>
    [XmlRoot("ReturnItemNotificationChargebackRecordMessage", Namespace = "Wonga.BankGateway.InternalMessages.Scotiabank.Ca", DataType = "" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages.Scotiabank.Ca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ReturnItemNotificationChargebackRecordMessage : MsmqMessage<ReturnItemNotificationChargebackRecordMessage>
    {
        public Object ReturnItemNotificationChargebackDetailsRecord { get; set; }
        public String Filename { get; set; }
        public Byte[] FileContents { get; set; }
    }
}
