using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.FileStorage.Queries
{
	[XmlRoot("GetLoanTopUpAgreement")]
	public partial class GetLoanTopUpAgreementQuery : ApiRequest<GetLoanTopUpAgreementQuery>
	{
		public Object AccountId { get; set; }
		public Object FixedTermLoanTopupId { get; set; }
	}
}
