using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.UserVerification
{
    /// <summary> Wonga.Risk.UserVerification.CounterOfferUserVerificationMessage </summary>
    [XmlRoot("CounterOfferUserVerificationMessage", Namespace = "Wonga.Risk.UserVerification", DataType = "")]
    public partial class CounterOfferUserVerificationMessage : MsmqMessage<CounterOfferUserVerificationMessage>
    {
        public Decimal Amount { get; set; }
        public Boolean IsNegative { get; set; }
        public Int32 RiskApplicationId { get; set; }
    }
}
