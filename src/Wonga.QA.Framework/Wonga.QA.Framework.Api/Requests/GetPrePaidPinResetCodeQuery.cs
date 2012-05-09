using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetPrePaidPinResetCode")]
    public partial class GetPrepaidResetCodeQuery : ApiRequest<GetPrepaidResetCodeQuery>
    {
        public Guid CustomerExternalId { get; set; }
    }
}
