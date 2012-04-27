using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.GetInstallmentQuote </summary>
    [XmlRoot("GetInstallmentQuote")]
    public partial class GetInstallmentQuoteQuery : ApiRequest<GetInstallmentQuoteQuery>
    {
        public Object MerchantId { get; set; }

        public Object TotalValue { get; set; }

        public Guid? PromoCodeId { get; set; }
    }
}
