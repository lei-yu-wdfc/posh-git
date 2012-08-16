using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.PayLater.Commands.Uk
{
	[XmlRoot("CreatePaylaterApplication")]
	public partial class CreatePaylaterApplicationUkCommand : ApiRequest<CreatePaylaterApplicationUkCommand>
	{
		public Object AccountId { get; set; }
		public Object ApplicationId { get; set; }
		public Object MerchantId { get; set; }
		public Object MerchantReference { get; set; }
		public Object MerchantOrderId { get; set; }
		public Object TotalAmount { get; set; }
		public Object Currency { get; set; }
		public Object PostCode { get; set; }
	}
}
