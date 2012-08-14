using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Marketing.Commands
{
    /// <summary> Wonga.Marketing.Commands.SaveCustomerFeedback </summary>
    [XmlRoot("SaveCustomerFeedback")]
    public partial class SaveCustomerFeedbackCommand : ApiRequest<SaveCustomerFeedbackCommand>
    {
        public Object AccountId { get; set; }
        public Object Feedback { get; set; }
    }
}
