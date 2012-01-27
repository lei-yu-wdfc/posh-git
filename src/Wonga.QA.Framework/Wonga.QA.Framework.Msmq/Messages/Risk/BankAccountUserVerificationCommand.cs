using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("BankAccountUserVerificationMessage", Namespace = "Wonga.Risk.UserVerification", DataType = "")]
    public class BankAccountUserVerificationCommand : MsmqMessage<BankAccountUserVerificationCommand>
    {
        public Int32 RiskApplicationId { get; set; }
    }
}
