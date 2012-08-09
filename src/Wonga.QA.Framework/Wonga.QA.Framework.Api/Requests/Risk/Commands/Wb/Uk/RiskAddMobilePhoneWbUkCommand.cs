using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Wb.Uk
{
    /// <summary> Wonga.Risk.Commands.Wb.Uk.RiskAddMobilePhone </summary>
    [XmlRoot("RiskAddMobilePhone")]
    public partial class RiskAddMobilePhoneWbUkCommand : ApiRequest<RiskAddMobilePhoneWbUkCommand>
    {
        public Object AccountId { get; set; }
        public Object MobilePhone { get; set; }
    }
}
