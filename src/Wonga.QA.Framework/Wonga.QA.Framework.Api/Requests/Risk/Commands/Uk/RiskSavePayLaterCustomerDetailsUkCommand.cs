using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.PayLater.Uk
{
	[XmlRoot("RiskSavePayLaterCustomerDetails")]
	public partial class RiskSavePayLaterCustomerDetailsUkCommand : ApiRequest<RiskSavePayLaterCustomerDetailsUkCommand>
	{
		public Object AccountId { get; set; }
		public Object Forename { get; set; }
		public Object Surname { get; set; }
		public Object Email { get; set; }
		public Object MobilePhone { get; set; }
		public Object DateOfBirth { get; set; }
	}
}
