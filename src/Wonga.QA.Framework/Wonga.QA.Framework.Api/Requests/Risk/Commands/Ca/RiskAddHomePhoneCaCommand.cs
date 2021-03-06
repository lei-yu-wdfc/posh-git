using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Ca
{
    /// <summary> Wonga.Risk.Commands.Ca.RiskAddHomePhone </summary>
    [XmlRoot("RiskAddHomePhone")]
    public partial class RiskAddHomePhoneCaCommand : ApiRequest<RiskAddHomePhoneCaCommand>
    {
        public Object AccountId { get; set; }
        public Object HomePhone { get; set; }
    }
}
