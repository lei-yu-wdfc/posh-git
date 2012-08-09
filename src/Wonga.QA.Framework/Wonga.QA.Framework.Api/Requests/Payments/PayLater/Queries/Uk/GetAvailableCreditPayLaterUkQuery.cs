using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Queries.Uk
{
    /// <summary> Wonga.Payments.PayLater.Queries.Uk.GetAvailableCredit </summary>
    [XmlRoot("GetAvailableCredit")]
    public partial class GetAvailableCreditPayLaterUkQuery : ApiRequest<GetAvailableCreditPayLaterUkQuery>
    {
        public Object AccountId { get; set; }
    }
}
