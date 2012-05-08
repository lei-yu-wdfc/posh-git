using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("SignupCustomerForPremiumCardCommand")]
    public partial class SignupCustomerForPremiumCardCommand : ApiRequest<SignupCustomerForPremiumCardCommand>
    {
        public Guid CustomerExternalId { get; set; }
    }
}
