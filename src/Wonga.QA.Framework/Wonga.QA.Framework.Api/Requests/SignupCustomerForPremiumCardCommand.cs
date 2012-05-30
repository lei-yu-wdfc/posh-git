using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Marketing.Commands.SignupCustomerForPremiumCardCommand </summary>
    [XmlRoot("SignupCustomerForPremiumCardCommand")]
    public partial class SignupCustomerForPremiumCardCommand : ApiRequest<SignupCustomerForPremiumCardCommand>
    {
        public Object CustomerExternalId { get; set; }
    }
}
