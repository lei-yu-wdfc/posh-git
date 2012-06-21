using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Queries.PLater.Uk
{
    /// <summary> Wonga.Risk.Queries.PLater.Uk.VerifyPaylaterCheckout </summary>
    [XmlRoot("VerifyPaylaterCheckout")]
    public partial class VerifyPaylaterCheckoutPLaterUkQuery : ApiRequest<VerifyPaylaterCheckoutPLaterUkQuery>
    {
        public Object AccountId { get; set; }
        public Object Application { get; set; }
    }
}
