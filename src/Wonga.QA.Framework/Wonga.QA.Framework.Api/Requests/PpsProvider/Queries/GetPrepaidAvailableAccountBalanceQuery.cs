using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.PpsProvider.Queries
{
    /// <summary> Wonga.PpsProvider.Queries.GetPrepaidAvailableAccountBalance </summary>
    [XmlRoot("GetPrepaidAvailableAccountBalance")]
    public partial class GetPrepaidAvailableAccountBalanceQuery : ApiRequest<GetPrepaidAvailableAccountBalanceQuery>
    {
        public Object AccountId { get; set; }
    }
}
