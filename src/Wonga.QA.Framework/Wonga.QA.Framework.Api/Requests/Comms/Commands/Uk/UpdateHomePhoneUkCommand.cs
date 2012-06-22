using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.Uk
{
    /// <summary> Wonga.Comms.Commands.Uk.UpdateHomePhone </summary>
    [XmlRoot("UpdateHomePhone")]
    public partial class UpdateHomePhoneUkCommand : ApiRequest<UpdateHomePhoneUkCommand>
    {
        public Object AccountId { get; set; }
        public Object HomePhone { get; set; }
    }
}