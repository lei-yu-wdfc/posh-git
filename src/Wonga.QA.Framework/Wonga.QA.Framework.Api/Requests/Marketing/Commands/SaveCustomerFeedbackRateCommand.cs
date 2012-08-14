using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Marketing.Commands
{
    /// <summary> Wonga.Marketing.Commands.SaveCustomerFeedbackRate </summary>
    [XmlRoot("SaveCustomerFeedbackRate")]
    public partial class SaveCustomerFeedbackRateCommand : ApiRequest<SaveCustomerFeedbackRateCommand>
    {
        public Object AccountId { get; set; }
        public Object Rate { get; set; }
    }
}
