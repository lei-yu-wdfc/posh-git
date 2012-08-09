using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Wb.Uk
{
    /// <summary> Wonga.Risk.Commands.Wb.Uk.RiskAddHomePhone </summary>
    [XmlRoot("RiskAddHomePhone")]
    public partial class RiskAddHomePhoneWbUkCommand : ApiRequest<RiskAddHomePhoneWbUkCommand>
    {
        public Object AccountId { get; set; }
        public Object HomePhone { get; set; }
    }
}
