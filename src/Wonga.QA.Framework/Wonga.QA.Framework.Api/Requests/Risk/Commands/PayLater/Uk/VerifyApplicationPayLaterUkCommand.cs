using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands.PayLater.Uk
{
    /// <summary> Wonga.Risk.Commands.PayLater.Uk.VerifyPaylaterApplication </summary>
    [XmlRoot("VerifyPaylaterApplication")]
    public partial class VerifyApplicationPayLaterUkCommand : ApiRequest<VerifyApplicationPayLaterUkCommand>
    {
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
    }
}
