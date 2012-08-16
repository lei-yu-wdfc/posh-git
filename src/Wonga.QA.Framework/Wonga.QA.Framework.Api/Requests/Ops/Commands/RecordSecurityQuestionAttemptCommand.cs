using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Ops.Commands
{
	[XmlRoot("RecordSecurityQuestionAttempt")]
	public partial class RecordSecurityQuestionAttemptCommand : ApiRequest<RecordSecurityQuestionAttemptCommand>
	{
		public Object FirstSecurityQuestionExternalId { get; set; }
		public Object SecondSecurityQuestionExternalId { get; set; }
		public Object FirstSecurityQuestionAnswer { get; set; }
		public Object SecondSecurityQuestionAnswer { get; set; }
	}
}
