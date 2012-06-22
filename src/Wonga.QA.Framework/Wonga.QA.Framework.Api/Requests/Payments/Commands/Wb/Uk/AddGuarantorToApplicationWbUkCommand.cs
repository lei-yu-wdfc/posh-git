using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands.Wb.Uk
{
    /// <summary> Wonga.Payments.Commands.Wb.Uk.AddGuarantorToApplication </summary>
    [XmlRoot("AddGuarantorToApplication")]
    public partial class AddGuarantorToApplicationWbUkCommand : ApiRequest<AddGuarantorToApplicationWbUkCommand>
    {
        public Object ApplicationId { get; set; }
        public Object GuarantorId { get; set; }
    }
}
