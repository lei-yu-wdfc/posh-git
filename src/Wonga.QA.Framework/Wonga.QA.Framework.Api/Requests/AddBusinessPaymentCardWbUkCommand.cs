using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("AddBusinessPaymentCard")]
    public partial class AddBusinessPaymentCardWbUkCommand : ApiRequest<AddBusinessPaymentCardWbUkCommand>
    {
        public Object OrganisationId { get; set; }
        public Object PaymentCardId { get; set; }
        public Object CardType { get; set; }
        public Object Number { get; set; }
        public Object HolderName { get; set; }
        public Object StartDate { get; set; }
        public Object ExpiryDate { get; set; }
        public Object IssueNo { get; set; }
        public Object SecurityCode { get; set; }
        public Object IsCreditCard { get; set; }
        public Object IsPrimary { get; set; }
        public Object AdditionalDetails { get; set; }
    }
}
