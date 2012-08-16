using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.FileStorage.Queries
{
	[XmlRoot("GetLoanExtensionAgreement")]
	public partial class GetLoanExtensionAgreementQuery : ApiRequest<GetLoanExtensionAgreementQuery>
	{
		public Object ExtensionId { get; set; }
	}
}
