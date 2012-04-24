using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetCustomerPrepaidEligibility")]
    public partial class GetCustomerPrePaidEligibilityQuery : ApiRequest<GetCustomerPrePaidEligibilityQuery>
    {
        public Guid AccountId { get; set; }
    }
}
