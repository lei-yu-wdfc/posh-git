using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Comms.Queries.GetExtensionTsAndCsDocument </summary>
    [XmlRoot("GetExtensionTsAndCsDocument")]
    public partial class GetExtensionTsAndCsDocumentQuery : ApiRequest<GetExtensionTsAndCsDocumentQuery>
    {
        public Object ExtensionId { get; set; }
    }
}
