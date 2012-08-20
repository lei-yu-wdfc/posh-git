using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Cs;

namespace Wonga.QA.Framework.CsApi.Requests.Sms.Csapi.Queries
{
	[XmlRoot("GetSmsMessages")]
	public partial class GetSmsMessagesQuery : CsRequest<GetSmsMessagesQuery>
	{
		public Object SmsMessageId { get; set; }
	}
}
