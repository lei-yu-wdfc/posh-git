using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Framework.CsApi.Requests.Address.Queries.Uk
{
	[XmlRoot("GetAddressByDescriptorId")]
	public partial class GetAddressByDescriptorIdUkQuery : CsRequest<GetAddressByDescriptorIdUkQuery>
	{
		public Object DescriptorId { get; set; }
	}
}
