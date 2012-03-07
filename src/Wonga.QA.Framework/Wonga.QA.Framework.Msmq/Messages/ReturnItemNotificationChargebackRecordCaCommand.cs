using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.Scotiabank.Ca.ReturnItemNotificationChargebackRecordMessage </summary>
    [XmlRoot("ReturnItemNotificationChargebackRecordMessage", Namespace = "Wonga.BankGateway.InternalMessages.Scotiabank.Ca", DataType = "")]
    public partial class ReturnItemNotificationChargebackRecordCaCommand : MsmqMessage<ReturnItemNotificationChargebackRecordCaCommand>
    {
        public Object ReturnItemNotificationChargebackDetailsRecord { get; set; }
        public String Filename { get; set; }
        public Byte[] FileContents { get; set; }
    }
}
