using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Queries.Uk
{
    /// <summary> Wonga.Payments.PayLater.Queries.Uk.GetOrderSummary </summary>
    [XmlRoot("GetOrderSummary")]
    public partial class GetOrderSummaryPayLaterUkQuery : ApiRequest<GetOrderSummaryPayLaterUkQuery>
    {
        public Object MerchantReference { get; set; }
        public Object MerchantOrderId { get; set; }
    }
}
