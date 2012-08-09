using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Queries.Uk
{
    /// <summary> Wonga.Payments.PayLater.Queries.Uk.GetRepaymentDetails </summary>
    [XmlRoot("GetRepaymentDetails")]
    public partial class GetRepaymentDetailsPayLaterUkQuery : ApiRequest<GetRepaymentDetailsPayLaterUkQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
