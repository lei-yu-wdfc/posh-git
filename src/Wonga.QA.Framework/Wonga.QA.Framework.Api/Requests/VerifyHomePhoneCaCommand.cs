using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Comms.Commands.Ca.VerifyHomePhone </summary>
    [XmlRoot("VerifyHomePhone")]
    public partial class VerifyHomePhoneCaCommand : ApiRequest<VerifyHomePhoneCaCommand>
    {
        public Object VerificationId { get; set; }
        public Object AccountId { get; set; }
        public Object HomePhone { get; set; }
        public Object Forename { get; set; }
    }
}
