using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Comms.Commands.Uk
{
    /// <summary> Wonga.Comms.Commands.Uk.SaveReviewDetails </summary>
    [XmlRoot("SaveReviewDetails")]
    public partial class SaveReviewDetailsUkCommand : ApiRequest<SaveReviewDetailsUkCommand>
    {
        public Object AccountId { get; set; }
        public Object DataIsReviewed { get; set; }
    }
}
