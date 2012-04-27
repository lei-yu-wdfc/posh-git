using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.GetAvailableCredit </summary>
    [XmlRoot("GetAvailableCredit")]
    public partial class GetAvailabelCreditQuery : ApiRequest<GetAvailabelCreditQuery>
    {
        public Object AccountId { get; set; }
    }
}
