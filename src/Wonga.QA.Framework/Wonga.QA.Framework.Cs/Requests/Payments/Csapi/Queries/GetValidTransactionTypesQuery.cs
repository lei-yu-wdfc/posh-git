using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Queries
{
    /// <summary> Wonga.Payments.Csapi.Queries.GetValidTransactionTypes </summary>
    [XmlRoot("GetValidTransactionTypes")]
    public partial class GetValidTransactionTypesQuery : CsRequest<GetValidTransactionTypesQuery>
    {
    }
}
