using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Risk.Queries.GetSocialDetails </summary>
    [XmlRoot("GetSocialDetails")]
    public partial class GetSocialDetailsQuery : ApiRequest<GetSocialDetailsQuery>
    {
        public Object AccountId { get; set; }
    }
}
