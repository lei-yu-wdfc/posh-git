using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.FileStorage.Queries
{
	[XmlRoot("GetLoanAgreement")]
	public partial class GetLoanAgreementQuery : ApiRequest<GetLoanAgreementQuery>
	{
		public Object ApplicationId { get; set; }
	}
}
