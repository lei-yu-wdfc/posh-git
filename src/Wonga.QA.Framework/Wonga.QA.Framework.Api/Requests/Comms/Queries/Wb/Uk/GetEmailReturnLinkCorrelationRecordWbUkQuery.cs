using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Comms.Queries.Wb.Uk
{
    /// <summary> Wonga.Comms.Queries.Wb.Uk.GetEmailReturnLinkCorrelationRecord </summary>
    [XmlRoot("GetEmailReturnLinkCorrelationRecord")]
    public partial class GetEmailReturnLinkCorrelationRecordWbUkQuery : ApiRequest<GetEmailReturnLinkCorrelationRecordWbUkQuery>
    {
        public Object CompositeIdentifier { get; set; }
    }
}
