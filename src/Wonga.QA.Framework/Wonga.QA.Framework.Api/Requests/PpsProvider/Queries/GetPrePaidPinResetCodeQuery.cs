using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.PpsProvider.Queries
{
    /// <summary> Wonga.PpsProvider.Queries.GetPrePaidPinResetCode </summary>
    [XmlRoot("GetPrePaidPinResetCode")]
    public partial class GetPrePaidPinResetCodeQuery : ApiRequest<GetPrePaidPinResetCodeQuery>
    {
        public Object CustomerExternalId { get; set; }
    }
}
