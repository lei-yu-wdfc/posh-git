using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("SetAccountPreference")]
    public class SetAccountPreferenceCommand : ApiRequest<SetAccountPreferenceCommand>
    {
        public Object AccountId { get; set; }
        public Object RemindBeforeEndLoan { get; set; }
    }
}
