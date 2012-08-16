using System;
using System.Xml.Serialization;

namespace Wonga.Risk.Queries.PayLater.Uk
{
	[XmlRoot(VerifyPaylaterCheckout)]
	public class VerifyPaylaterCheckoutUkQuery : ApiRequest<VerifyPaylaterCheckoutUkQuery>
	{
		public Object AccountId { get; set; }
		public Object Application { get; set; }
	}
}
