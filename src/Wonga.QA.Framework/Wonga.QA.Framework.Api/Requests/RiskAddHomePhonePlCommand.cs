using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Risk.Commands.Pl.RiskAddHomePhone </summary>
    [XmlRoot("RiskAddHomePhone")]
    public partial class RiskAddHomePhonePlCommand : ApiRequest<RiskAddHomePhonePlCommand>
    {
        public Object AccountId { get; set; }
        public Object HomePhone { get; set; }
    }
}
