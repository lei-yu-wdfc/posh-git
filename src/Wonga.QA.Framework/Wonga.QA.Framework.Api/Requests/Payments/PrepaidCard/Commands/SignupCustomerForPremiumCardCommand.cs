using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.PrepaidCard.Commands
{
    /// <summary> Wonga.Payments.PrepaidCard.Commands.SignupCustomerForPremiumCardCommand </summary>
    [XmlRoot("SignupCustomerForPremiumCardCommand")]
    public partial class SignupCustomerForPremiumCardCommand : ApiRequest<SignupCustomerForPremiumCardCommand>
    {
        public Object CustomerExternalId { get; set; }
    }
}
