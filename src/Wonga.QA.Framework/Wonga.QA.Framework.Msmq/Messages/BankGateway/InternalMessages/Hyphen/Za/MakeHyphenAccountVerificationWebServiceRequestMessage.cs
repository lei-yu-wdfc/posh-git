using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Hyphen.Za
{
    /// <summary> Wonga.BankGateway.InternalMessages.Hyphen.Za.MakeHyphenAccountVerificationWebServiceRequestMessage </summary>
    [XmlRoot("MakeHyphenAccountVerificationWebServiceRequestMessage", Namespace = "Wonga.BankGateway.InternalMessages.Hyphen.Za", DataType = "")]
    public partial class MakeHyphenAccountVerificationWebServiceRequestMessage : MsmqMessage<MakeHyphenAccountVerificationWebServiceRequestMessage>
    {
        public Int32 BankAccountVerificationId { get; set; }
        public String AccountNumber { get; set; }
        public String BranchCode { get; set; }
        public String AccountHolderNationalNumber { get; set; }
        public String AccountHolderLastName { get; set; }
    }
}
