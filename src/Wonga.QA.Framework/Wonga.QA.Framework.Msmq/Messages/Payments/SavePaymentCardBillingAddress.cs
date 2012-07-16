using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments
{
    /// <summary> Wonga.Payments.SavePaymentCardBillingAddress </summary>
    [XmlRoot("SavePaymentCardBillingAddress", Namespace = "Wonga.Payments", DataType = "")]
    public partial class SavePaymentCardBillingAddress : MsmqMessage<SavePaymentCardBillingAddress>
    {
        public Guid PaymentCardId { get; set; }
        public String Flat { get; set; }
        public String HouseName { get; set; }
        public String HouseNumber { get; set; }
        public String Street { get; set; }
        public String District { get; set; }
        public String Town { get; set; }
        public String County { get; set; }
        public String CountryCode { get; set; }
        public String PostCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
