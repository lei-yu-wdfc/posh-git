using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.Uk
{
    /// <summary> Wonga.Comms.Commands.Uk.VerifyMobilePhone </summary>
    [XmlRoot("VerifyMobilePhone")]
    public partial class VerifyMobilePhoneUkCommand : ApiRequest<VerifyMobilePhoneUkCommand>
    {
        public Object VerificationId { get; set; }
        public Object AccountId { get; set; }
        public Object MobilePhone { get; set; }
        public Object Forename { get; set; }
    }
}
