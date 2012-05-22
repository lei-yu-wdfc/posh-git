using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Marketing.Commands.SignupCustomerForStandardCardCommand </summary>
    [XmlRoot("SignupCustomerForStandardCardCommand")]
    public partial class SignupCustomerForStandardCardCommand : ApiRequest<SignupCustomerForStandardCardCommand>
    {
        public Object CustomerExternalId { get; set; }
    }
}
