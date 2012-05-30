using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.PrepaidCard.Queries.GetPrepaidAvailableAccountBalanceQuery </summary>
    [XmlRoot("GetPrepaidAvailableAccountBalance")]
    public partial class GetPrepaidAvailableAccountBalanceQuery : ApiRequest<GetPrepaidAvailableAccountBalanceQuery>
    {
        public Object CustomerExternalId { get; set; }
    }
}
