using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Comms.Queries.Uk.GetAddressByDescriptorId </summary>
    [XmlRoot("GetAddressByDescriptorId")]
    public partial class GetAddressByDescriptorIdUkQuery : ApiRequest<GetAddressByDescriptorIdUkQuery>
    {
        public Object DescriptorId { get; set; }
    }
}
