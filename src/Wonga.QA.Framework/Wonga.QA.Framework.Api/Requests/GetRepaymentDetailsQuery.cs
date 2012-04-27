using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests
{
    /// <summary> Wonga.Payments.Queries.GetRepaymentDetails </summary>
    [XmlRoot("GetRepaymentDetails")]
    public partial class GetRepaymentDetailsQuery : ApiRequest<GetRepaymentDetailsQuery>
    {
        public Guid ApplicationId { get; set; }
    }
}
