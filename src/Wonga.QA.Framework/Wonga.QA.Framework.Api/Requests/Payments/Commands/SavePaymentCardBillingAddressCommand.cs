using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands
{
	[XmlRoot("SavePaymentCardBillingAddress")]
	public partial class SavePaymentCardBillingAddressCommand : ApiRequest<SavePaymentCardBillingAddressCommand>
	{
		public Object PaymentCardId { get; set; }
		public Object Flat { get; set; }
		public Object HouseName { get; set; }
		public Object HouseNumber { get; set; }
		public Object Street { get; set; }
		public Object District { get; set; }
		public Object Town { get; set; }
		public Object County { get; set; }
		public Object CountryCode { get; set; }
		public Object PostCode { get; set; }
	}
}
