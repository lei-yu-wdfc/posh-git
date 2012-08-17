using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.PayLater.Uk
{
    [XmlRoot("RiskCreatePayLaterApplication")]
    public partial class RiskCreatePayLaterApplicationUkCommand : ApiRequest<RiskCreatePayLaterApplicationUkCommand>
    {
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
        public Object PaymentCardId { get; set; }
        public Object TotalAmount { get; set; }
    }
}