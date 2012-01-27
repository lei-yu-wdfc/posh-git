using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("SaveContactPreferences")]
    public class SaveContactPreferencesCommand : ApiRequest<SaveContactPreferencesCommand>
    {
        public Object AccountId { get; set; }
        public Object AcceptMarketingContact { get; set; }
    }
}
