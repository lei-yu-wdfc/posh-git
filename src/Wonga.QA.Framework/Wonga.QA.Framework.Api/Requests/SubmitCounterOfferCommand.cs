using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Commands.SubmitCounterOffer </summary>
    [XmlRoot("SubmitCounterOffer")]
    public partial class SubmitCounterOfferCommand : ApiRequest<SubmitCounterOfferCommand>
    {
        public Object ApplicationId { get; set; }
        public Object UserActionId { get; set; }
        public Object NewLoanAmount { get; set; }
    }
}
