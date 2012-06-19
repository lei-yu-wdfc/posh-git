using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.PLater.Uk.GetCreditInUse </summary>
    [XmlRoot("GetCreditInUse")]
    public partial class GetCreditInUsePLaterUkQuery : ApiRequest<GetCreditInUsePLaterUkQuery>
    {
        public Object AccountId { get; set; }
    }
}
