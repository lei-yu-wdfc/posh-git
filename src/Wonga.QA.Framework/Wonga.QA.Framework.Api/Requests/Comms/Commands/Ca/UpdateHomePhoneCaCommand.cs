using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.Ca
{
    /// <summary> Wonga.Comms.Commands.Ca.UpdateHomePhone </summary>
    [XmlRoot("UpdateHomePhone")]
    public partial class UpdateHomePhoneCaCommand : ApiRequest<UpdateHomePhoneCaCommand>
    {
        public Object AccountId { get; set; }
        public Object HomePhone { get; set; }
    }
}
