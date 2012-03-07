using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    /// <summary> Wonga.Risk.UserVerification.BankAccountUserVerificationMessage </summary>
    [XmlRoot("BankAccountUserVerificationMessage", Namespace = "Wonga.Risk.UserVerification", DataType = "")]
    public partial class BankAccountUserVerificationCommand : MsmqMessage<BankAccountUserVerificationCommand>
    {
        public Int32 RiskApplicationId { get; set; }
    }
}
