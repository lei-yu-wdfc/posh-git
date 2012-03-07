using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Commands.Wb.Uk.SignBusinessApplication </summary>
    [XmlRoot("SignBusinessApplication")]
    public partial class SignBusinessApplicationWbUkCommand : ApiRequest<SignBusinessApplicationWbUkCommand>
    {
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
    }
}
