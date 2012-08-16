using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.PayLater.Uk
{
	[XmlRoot("SavePayLaterCustomerDetails")]
	public partial class SavePayLaterCustomerDetailsUkCommand : ApiRequest<SavePayLaterCustomerDetailsUkCommand>
	{
		public Object AccountId { get; set; }
		public Object DateOfBirth { get; set; }
		public Object Title { get; set; }
		public Object Forename { get; set; }
		public Object Surname { get; set; }
		public Object MobilePhone { get; set; }
		public Object Email { get; set; }
	}
}
