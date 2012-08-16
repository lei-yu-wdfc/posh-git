using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Address.Queries.Uk
{
	[XmlRoot("GetAddressDescriptorsByPostCode")]
	public partial class GetAddressDescriptorsByPostCodeUkQuery : ApiRequest<GetAddressDescriptorsByPostCodeUkQuery>
	{
		public Object Postcode { get; set; }
		public Object CountryCode { get; set; }
	}
}
