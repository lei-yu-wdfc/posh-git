using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.Za
{
    /// <summary> Wonga.Comms.Commands.Za.UpdateHomePhoneZa </summary>
    [XmlRoot("UpdateHomePhoneZa")]
    public partial class UpdateHomePhoneZaCommand : ApiRequest<UpdateHomePhoneZaCommand>
    {
        public Object AccountId { get; set; }
        public Object HomePhone { get; set; }
    }
}
