using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("SignBusinessApplication", Namespace = "Wonga.Payments.Commands.Wb.Uk", DataType = "")]
    public class SignBusinessApplicationWbUkCommand : MsmqMessage<SignBusinessApplicationWbUkCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
