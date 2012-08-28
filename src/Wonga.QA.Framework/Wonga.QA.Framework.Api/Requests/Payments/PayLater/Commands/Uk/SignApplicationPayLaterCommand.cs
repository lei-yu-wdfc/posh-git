using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Commands.Uk
{
    [XmlRoot("SignPayLaterApplication")]
    public partial class SignApplicationPayLaterCommand : ApiRequest<SignApplicationPayLaterCommand>
    {
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
    }
}
