using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.UserVerification
{
    /// <summary> Wonga.Risk.UserVerification.BankAccountUserVerificationMessage </summary>
    [XmlRoot("BankAccountUserVerificationMessage", Namespace = "Wonga.Risk.UserVerification", DataType = "")]
    public partial class BankAccountUserVerificationMessage : MsmqMessage<BankAccountUserVerificationMessage>
    {
        public Int32 RiskApplicationId { get; set; }
    }
}
