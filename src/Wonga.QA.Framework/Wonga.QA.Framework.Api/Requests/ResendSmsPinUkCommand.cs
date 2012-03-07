using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Comms.Commands.Uk.ResendSmsPin </summary>
    [XmlRoot("ResendSmsPin")]
    public partial class ResendSmsPinUkCommand : ApiRequest<ResendSmsPinUkCommand>
    {
        public Object AccountId { get; set; }
        public Object MobilePhone { get; set; }
    }
}
