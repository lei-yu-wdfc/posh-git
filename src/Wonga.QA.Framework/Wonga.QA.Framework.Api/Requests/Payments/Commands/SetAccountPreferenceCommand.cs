using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
    /// <summary> Wonga.Payments.Commands.SetAccountPreference </summary>
    [XmlRoot("SetAccountPreference")]
    public partial class SetAccountPreferenceCommand : ApiRequest<SetAccountPreferenceCommand>
    {
        public Object AccountId { get; set; }
        public Object RemindBeforeEndLoan { get; set; }
    }
}
