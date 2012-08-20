using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Framework.CsApi.Requests.Address.Queries.Uk
{
	[XmlRoot("GetAddressDescriptorsByPostCode")]
	public partial class GetAddressDescriptorsByPostCodeUkQuery : CsRequest<GetAddressDescriptorsByPostCodeUkQuery>
	{
		public Object Postcode { get; set; }
		public Object CountryCode { get; set; }
	}
}
