using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Ops.Queries
{
	[XmlRoot("GetSecurityQuestionsDecision")]
	public partial class GetSecurityQuestionsDecisionQuery : ApiRequest<GetSecurityQuestionsDecisionQuery>
	{
		public Object FirstSecurityQuestionExternalId { get; set; }
		public Object SecondSecurityQuestionExternalId { get; set; }
	}
}
