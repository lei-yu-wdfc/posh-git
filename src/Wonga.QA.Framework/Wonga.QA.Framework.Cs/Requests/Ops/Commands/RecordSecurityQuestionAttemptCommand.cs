using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Framework.CsApi.Requests.Ops.Commands
{
	[XmlRoot("RecordSecurityQuestionAttempt")]
	public partial class RecordSecurityQuestionAttemptCommand : CsRequest<RecordSecurityQuestionAttemptCommand>
	{
		public Object FirstSecurityQuestionExternalId { get; set; }
		public Object SecondSecurityQuestionExternalId { get; set; }
		public Object FirstSecurityQuestionAnswer { get; set; }
		public Object SecondSecurityQuestionAnswer { get; set; }
	}
}
