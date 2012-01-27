using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("CompleteEmailVerification")]
    public class CompleteEmailVerificationCommand : ApiRequest<CompleteEmailVerificationCommand>
    {
        public Object AccountId { get; set; }
        public Object ChangeId { get; set; }
    }
}
