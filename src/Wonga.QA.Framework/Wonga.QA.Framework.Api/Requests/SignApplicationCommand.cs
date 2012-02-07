using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("SignApplication")]
    public partial class SignApplicationCommand : ApiRequest<SignApplicationCommand>
    {
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
    }
}
