using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.Uk
{
	[XmlRoot("RiskSaveCustomerDetails")]
	public partial class RiskSaveCustomerDetailsUkCommand : ApiRequest<RiskSaveCustomerDetailsUkCommand>
	{
		public Object AccountId { get; set; }
		public Object Gender { get; set; }
		public Object Forename { get; set; }
		public Object Surname { get; set; }
		public Object MiddleName { get; set; }
		public Object HomePhone { get; set; }
		public Object WorkPhone { get; set; }
		public Object Email { get; set; }
		public Object MobilePhone { get; set; }
		public Object DateOfBirth { get; set; }
		public Object MaidenName { get; set; }
	}
}
