using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands.PLater.Uk
{
    /// <summary> Wonga.Payments.Commands.PLater.Uk.TakePayLaterEarlyRepayment </summary>
    [XmlRoot("TakePayLaterEarlyRepayment")]
    public partial class TakePayLaterEarlyRepaymentUkCommand : ApiRequest<TakePayLaterEarlyRepaymentUkCommand>
    {
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
        public Object PaymentRequestId { get; set; }
        public Object PaymentCardId { get; set; }
        public Object Cv2 { get; set; }
        public Object Amount { get; set; }
    }
}
