using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetContactPreferences")]
    public class GetContactPreferencesQuery : ApiRequest<GetContactPreferencesQuery>
    {
        public Object AccountId { get; set; }
    }
}
