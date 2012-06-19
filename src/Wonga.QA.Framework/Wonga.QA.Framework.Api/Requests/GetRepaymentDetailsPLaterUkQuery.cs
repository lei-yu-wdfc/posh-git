using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.PLater.Uk.GetRepaymentDetails </summary>
    [XmlRoot("GetRepaymentDetails")]
    public partial class GetRepaymentDetailsPLaterUkQuery : ApiRequest<GetRepaymentDetailsPLaterUkQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
