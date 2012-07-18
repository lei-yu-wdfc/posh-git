using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.PLater.Uk
{
    /// <summary> Wonga.Payments.Queries.PLater.Uk.GetRepaymentDetails </summary>
    [XmlRoot("GetRepaymentDetails")]
    public partial class GetRepaymentDetailsUkQuery : ApiRequest<GetRepaymentDetailsUkQuery>
    {
        public Object ApplicationId { get; set; }
    }
}