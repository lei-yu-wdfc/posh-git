using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Address.Queries.Uk
{
	[XmlRoot("GetAddressByDescriptorId")]
	public partial class GetAddressByDescriptorIdUkQuery : ApiRequest<GetAddressByDescriptorIdUkQuery>
	{
		public Object DescriptorId { get; set; }
	}
}
