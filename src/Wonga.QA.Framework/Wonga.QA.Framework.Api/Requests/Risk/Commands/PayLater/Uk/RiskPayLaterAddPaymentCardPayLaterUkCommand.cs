using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.PayLater.Uk
{
    /// <summary> Wonga.Risk.Commands.PayLater.Uk.RiskPayLaterAddPaymentCard </summary>
    [XmlRoot("RiskPayLaterAddPaymentCard")]
    public partial class RiskPayLaterAddPaymentCardPayLaterUkCommand : ApiRequest<RiskPayLaterAddPaymentCardPayLaterUkCommand>
    {
        public Object AccountId { get; set; }
        public Object PaymentCardId { get; set; }
        public Object Number { get; set; }
        public Object CardType { get; set; }
        public Object ExpiryDate { get; set; }
        public Object SecurityCode { get; set; }
    }
}
