using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("CounterOfferUserVerificationMessage", Namespace = "Wonga.Risk.UserVerification", DataType = "")]
    public class CounterOfferUserVerificationCommand : MsmqMessage<CounterOfferUserVerificationCommand>
    {
        public Decimal Amount { get; set; }
        public Boolean IsNegative { get; set; }
        public Int32 RiskApplicationId { get; set; }
    }
}
