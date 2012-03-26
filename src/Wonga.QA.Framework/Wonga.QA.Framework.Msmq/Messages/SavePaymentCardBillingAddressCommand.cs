using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.SavePaymentCardBillingAddress </summary>
    [XmlRoot("SavePaymentCardBillingAddress", Namespace = "Wonga.Payments", DataType = "")]
    public partial class SavePaymentCardBillingAddressCommand : MsmqMessage<SavePaymentCardBillingAddressCommand>
    {
        public Guid PaymentCardId { get; set; }
        public String AddressLine1 { get; set; }
        public String AddressLine2 { get; set; }
        public String Town { get; set; }
        public String County { get; set; }
        public String CountryCode { get; set; }
        public String PostCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
