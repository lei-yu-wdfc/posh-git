using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.PayLater.Uk
{
	[XmlRoot("RiskSavePayLaterEmploymentDetails")]
	public partial class RiskSavePayLaterEmploymentDetailsUkCommand : ApiRequest<RiskSavePayLaterEmploymentDetailsUkCommand>
	{
		public Object AccountId { get; set; }
		public Object EmploymentStatus { get; set; }
		public Object IncomeFrequency { get; set; }
		public Object NetIncome { get; set; }
		public Object NextPayDate { get; set; }
	}
}
