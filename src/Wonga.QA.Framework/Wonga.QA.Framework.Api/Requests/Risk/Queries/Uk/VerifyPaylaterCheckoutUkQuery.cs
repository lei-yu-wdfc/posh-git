using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Risk.Queries.PayLater.Uk
{
	[XmlRoot("VerifyPaylaterCheckout")]
	public partial class VerifyPaylaterCheckoutUkQuery : ApiRequest<VerifyPaylaterCheckoutUkQuery>
	{
		public Object AccountId { get; set; }
		public Object Application { get; set; }
	}
}
