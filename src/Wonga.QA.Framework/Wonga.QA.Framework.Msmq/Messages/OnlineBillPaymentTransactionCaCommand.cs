using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.Scotiabank.Ca.OnlineBillPaymentTransaction </summary>
    [XmlRoot("OnlineBillPaymentTransaction", Namespace = "Wonga.BankGateway.InternalMessages.Scotiabank.Ca", DataType = "")]
    public partial class OnlineBillPaymentTransactionCaCommand : MsmqMessage<OnlineBillPaymentTransactionCaCommand>
    {
        public String RecordType { get; set; }
        public Int32 ItemNumber { get; set; }
        public Int32 BatchNumber { get; set; }
        public Int64 Amount { get; set; }
        public String Ccin { get; set; }
        public DateTime RemittancePaymentDate { get; set; }
        public String RemittanceTraceNumber { get; set; }
        public String CustomerFullName { get; set; }
        public String Filename { get; set; }
        public Byte[] FileContents { get; set; }
    }
}
