using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Payments.Queries
{
	[XmlRoot("GetBankHolidays")]
	public partial class GetBankHolidaysQuery : ApiRequest<GetBankHolidaysQuery>
	{
	}
}
