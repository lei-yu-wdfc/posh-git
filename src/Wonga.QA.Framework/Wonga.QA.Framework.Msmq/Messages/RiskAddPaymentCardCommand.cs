using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.RiskAddPaymentCard </summary>
    [XmlRoot("RiskAddPaymentCard", Namespace = "Wonga.Risk", DataType = "")]
    public partial class RiskAddPaymentCardCommand : MsmqMessage<RiskAddPaymentCardCommand>
    {
        public Guid AccountId { get; set; }
        public Guid PaymentCardId { get; set; }
        public String CardType { get; set; }
        public String Number { get; set; }
        public String HolderName { get; set; }
        public String SecurityCode { get; set; }
        public String StartDateXml { get; set; }
        public String ExpiryDateXml { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
