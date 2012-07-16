using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.CardPayment.InternalMessages
{
    /// <summary> Wonga.CardPayment.InternalMessages.SetServiceLoginPrimaryMessage </summary>
    [XmlRoot("SetServiceLoginPrimaryMessage", Namespace = "Wonga.CardPayment.InternalMessages", DataType = "")]
    public partial class SetServiceLoginPrimaryMessage : MsmqMessage<SetServiceLoginPrimaryMessage>
    {
        public Guid ServiceLoginId { get; set; }
    }
}
