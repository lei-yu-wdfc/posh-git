using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.Uk
{
	[XmlRoot("SaveReviewDetails")]
	public partial class SaveReviewDetailsUkCommand : ApiRequest<SaveReviewDetailsUkCommand>
	{
		public Object AccountId { get; set; }
		public Object DataIsReviewed { get; set; }
	}
}
