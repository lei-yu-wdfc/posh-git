using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.PLater.Uk.GetAvailableCredit </summary>
    [XmlRoot("GetAvailableCredit")]
    public partial class GetAvailableCreditPLaterUkQuery : ApiRequest<GetAvailableCreditPLaterUkQuery>
    {
        public Object AccountId { get; set; }
    }
}
