using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Ops.Queries
{
    /// <summary> Wonga.Ops.Queries.GetPasswordRecoveryDetails </summary>
    [XmlRoot("GetPasswordRecoveryDetails")]
    public partial class GetPasswordRecoveryDetailsQuery : ApiRequest<GetPasswordRecoveryDetailsQuery>
    {
        public Object AccountId { get; set; }
    }
}
