using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Risk.Commands.Uk.RiskAddHomePhone </summary>
    [XmlRoot("RiskAddHomePhone")]
    public partial class RiskAddHomePhoneUkCommand : ApiRequest<RiskAddHomePhoneUkCommand>
    {
        public Object AccountId { get; set; }
        public Object HomePhone { get; set; }
    }
}
