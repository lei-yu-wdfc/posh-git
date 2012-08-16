using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.PayLater.Uk
{
	[XmlRoot("RiskSavePayLaterCustomerAddress")]
	public partial class RiskSavePayLaterCustomerAddressUkCommand : ApiRequest<RiskSavePayLaterCustomerAddressUkCommand>
	{
		public Object AddressId { get; set; }
		public Object AccountId { get; set; }
		public Object Flat { get; set; }
		public Object HouseNumber { get; set; }
		public Object Street { get; set; }
		public Object Town { get; set; }
		public Object Postcode { get; set; }
	}
}
