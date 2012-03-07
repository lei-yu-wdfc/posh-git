using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.PostAccruedInterest </summary>
    [XmlRoot("PostAccruedInterest", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class PostAccruedInterestCommand : MsmqMessage<PostAccruedInterestCommand>
    {
        public Int32 ApplicationId { get; set; }
    }
}
