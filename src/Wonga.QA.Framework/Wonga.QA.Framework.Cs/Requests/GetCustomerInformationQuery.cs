using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Comms.Csapi.Queries.GetCustomerInformation </summary>
    [XmlRoot("GetCustomerInformation")]
    public partial class GetCustomerInformationQuery : CsRequest<GetCustomerInformationQuery>
    {
        public Object AccountId { get; set; }
    }
}
