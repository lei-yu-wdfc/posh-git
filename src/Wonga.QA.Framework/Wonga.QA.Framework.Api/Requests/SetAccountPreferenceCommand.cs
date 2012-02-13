using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("SetAccountPreference")]
    public partial class SetAccountPreferenceCommand : ApiRequest<SetAccountPreferenceCommand>
    {
        public Object AccountId { get; set; }
        public Object RemindBeforeEndLoan { get; set; }
    }
}
