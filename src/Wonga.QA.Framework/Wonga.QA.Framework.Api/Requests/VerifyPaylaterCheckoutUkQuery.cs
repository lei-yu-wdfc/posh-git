using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Risk.Queries.PLater.Uk.VerifyPaylaterCheckout </summary>
    [XmlRoot("VerifyPaylaterCheckout")]
    public partial class VerifyPaylaterCheckoutUkQuery : ApiRequest<VerifyPaylaterCheckoutUkQuery>
    {
        public Object AccountId { get; set; }
        public Object Application { get; set; }
    }
}
