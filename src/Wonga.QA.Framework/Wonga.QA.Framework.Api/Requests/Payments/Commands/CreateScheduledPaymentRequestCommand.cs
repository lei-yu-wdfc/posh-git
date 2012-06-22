using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
    /// <summary> Wonga.Payments.Commands.CreateScheduledPaymentRequest </summary>
    [XmlRoot("CreateScheduledPaymentRequest")]
    public partial class CreateScheduledPaymentRequestCommand : ApiRequest<CreateScheduledPaymentRequestCommand>
    {
        public Object ApplicationId { get; set; }
        public Object ExtensionId { get; set; }
    }
}
