using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Queries.GetRepaymentArrangementAllowed </summary>
    [XmlRoot("GetRepaymentArrangementAllowed")]
    public partial class GetRepaymentArrangementAllowedQuery : CsRequest<GetRepaymentArrangementAllowedQuery>
    {
        public Object ApplicationId { get; set; }
    }
}
