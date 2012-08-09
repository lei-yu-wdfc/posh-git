using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Queries.Uk
{
    /// <summary> Wonga.Payments.PayLater.Queries.Uk.GetCreditInUse </summary>
    [XmlRoot("GetCreditInUse")]
    public partial class GetCreditInUsePayLaterUkQuery : ApiRequest<GetCreditInUsePayLaterUkQuery>
    {
        public Object AccountId { get; set; }
    }
}
