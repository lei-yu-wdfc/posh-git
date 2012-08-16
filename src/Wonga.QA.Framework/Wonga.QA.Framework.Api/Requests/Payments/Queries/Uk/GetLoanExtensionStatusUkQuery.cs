using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries.Uk
{
	[XmlRoot("GetLoanExtensionStatus")]
	public partial class GetLoanExtensionStatusUkQuery : ApiRequest<GetLoanExtensionStatusUkQuery>
	{
		public Object ExtensionId { get; set; }
	}
}
