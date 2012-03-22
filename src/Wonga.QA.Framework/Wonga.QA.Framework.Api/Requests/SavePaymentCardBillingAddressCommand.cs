using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Commands.SavePaymentCardBillingAddress </summary>
    [XmlRoot("SavePaymentCardBillingAddress")]
    public partial class SavePaymentCardBillingAddressCommand : ApiRequest<SavePaymentCardBillingAddressCommand>
    {
        public Object PaymentCardId { get; set; }
        public Object AddressLine1 { get; set; }
        public Object AddressLine2 { get; set; }
        public Object Town { get; set; }
        public Object County { get; set; }
        public Object CountryCode { get; set; }
        public Object PostCode { get; set; }
    }
}
