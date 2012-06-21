using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.PrepaidCard.Commands
{
    /// <summary> Wonga.Payments.PrepaidCard.Commands.SignupCustomerForStandardCardCommand </summary>
    [XmlRoot("SignupCustomerForStandardCardCommand")]
    public partial class SignupCustomerForStandardCardCommand : ApiRequest<SignupCustomerForStandardCardCommand>
    {
        public Object CustomerExternalId { get; set; }
    }
}
