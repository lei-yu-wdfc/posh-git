using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("GetEmailReturnLinkCorrelationRecord")]
    public partial class GetEmailReturnLinkCorrelationRecordWbUkQuery : ApiRequest<GetEmailReturnLinkCorrelationRecordWbUkQuery>
    {
        public Object CompositeIdentifier { get; set; }
    }
}
