using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.PrepaidCard.Queries.GetPrePaidPinResetCode </summary>
    [XmlRoot("GetPrePaidPinResetCode")]
    public partial class GetPrePaidPinResetCodeQuery : ApiRequest<GetPrePaidPinResetCodeQuery>
    {
        public Object CustomerExternalId { get; set; }
    }
}
