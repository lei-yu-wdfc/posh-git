using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.FileStorage.Queries.GetExtensionAEDocument </summary>
    [XmlRoot("GetExtensionAEDocument")]
    public partial class GetExtensionAeDocumentQuery : ApiRequest<GetExtensionAeDocumentQuery>
    {
        public Object ExtensionId { get; set; }
    }
}
