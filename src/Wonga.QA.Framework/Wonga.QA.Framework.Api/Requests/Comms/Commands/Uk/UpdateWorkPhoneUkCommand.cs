using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.Uk
{
    /// <summary> Wonga.Comms.Commands.Uk.UpdateWorkPhone </summary>
    [XmlRoot("UpdateWorkPhone")]
    public partial class UpdateWorkPhoneUkCommand : ApiRequest<UpdateWorkPhoneUkCommand>
    {
        public Object AccountId { get; set; }
        public Object WorkPhone { get; set; }
    }
}
