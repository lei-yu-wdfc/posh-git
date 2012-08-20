using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Framework.CsApi.Requests.Ops.Integration.Merchants.CsApi.Commands.Uk
{
	[XmlRoot("CsUpdateMerchantAccount")]
	public partial class CsUpdateMerchantAccountUkCommand : CsRequest<CsUpdateMerchantAccountUkCommand>
	{
		public Object Name { get; set; }
		public Object MerchantId { get; set; }
		public Object PhysicalAddress { get; set; }
		public Object StartDate { get; set; }
		public Object CategoryCode { get; set; }
	}
}
