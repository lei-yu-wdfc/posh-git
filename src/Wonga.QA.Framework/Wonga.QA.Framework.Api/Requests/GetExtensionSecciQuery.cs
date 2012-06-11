using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.FileStorage.Queries.GetExtensionSecci </summary>
    [XmlRoot("GetExtensionSecci")]
    public partial class GetExtensionSecciQuery : ApiRequest<GetExtensionSecciQuery>
    {
        public Object ExtensionId { get; set; }
    }
}
