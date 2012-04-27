using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Queries.GetApplicationApr</summary>
    [XmlRoot("GetApplicationApr")]
    public partial class GetApplicationAprQuery : ApiRequest<GetApplicationAprQuery>
    {
        public Guid MerchantId { get; set; }

        public decimal TotalValue { get; set; }

        public Guid? PromoCodeId { get; set; }
    }
}
