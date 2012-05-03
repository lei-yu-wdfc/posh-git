using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("SignupCustomerForStandardCardCommand")]
    public partial class SignupCustomerForStandardCardCommand : ApiRequest<SignupCustomerForStandardCardCommand>
    {
        public Guid CustomerExternalId { get; set; }
    }
}
