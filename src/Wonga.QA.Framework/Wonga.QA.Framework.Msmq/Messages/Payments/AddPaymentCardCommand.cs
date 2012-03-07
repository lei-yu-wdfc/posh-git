using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    /// <summary> Wonga.Payments.AddPaymentCard </summary>
    [XmlRoot("AddPaymentCard", Namespace = "Wonga.Payments", DataType = "")]
    public partial class AddPaymentCardCommand : MsmqMessage<AddPaymentCardCommand>
    {
        public Guid AccountId { get; set; }
        public Guid PaymentCardId { get; set; }
        public String CardType { get; set; }
        public String Number { get; set; }
        public String HolderName { get; set; }
        public String IssueNo { get; set; }
        public String SecurityCode { get; set; }
        public Boolean IsCreditCard { get; set; }
        public Boolean IsPrimary { get; set; }
        public Object AdditionalDetails { get; set; }
        public String StartDateXml { get; set; }
        public String ExpiryDateXml { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
