using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Payments
{
    [XmlRoot("PostAccruedInterest", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class PostAccruedInterestCommand : MsmqMessage<PostAccruedInterestCommand>
    {
        public Int32 ApplicationId { get; set; }
    }
}
