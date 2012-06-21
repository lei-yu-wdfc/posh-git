using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Comms.Queries
{
    /// <summary> Wonga.Comms.Queries.GetContactPreferences </summary>
    [XmlRoot("GetContactPreferences")]
    public partial class GetContactPreferencesQuery : ApiRequest<GetContactPreferencesQuery>
    {
        public Object AccountId { get; set; }
    }
}
