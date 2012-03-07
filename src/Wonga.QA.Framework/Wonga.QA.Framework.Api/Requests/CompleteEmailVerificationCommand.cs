using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Comms.Commands.CompleteEmailVerification </summary>
    [XmlRoot("CompleteEmailVerification")]
    public partial class CompleteEmailVerificationCommand : ApiRequest<CompleteEmailVerificationCommand>
    {
        public Object AccountId { get; set; }
        public Object ChangeId { get; set; }
    }
}
