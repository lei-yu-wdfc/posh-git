using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Risk.Commands.Uk.RiskAddMobilePhone </summary>
    [XmlRoot("RiskAddMobilePhone")]
    public partial class RiskAddMobilePhoneUkCommand : ApiRequest<RiskAddMobilePhoneUkCommand>
    {
        public Object AccountId { get; set; }
        public Object MobilePhone { get; set; }
    }
}
