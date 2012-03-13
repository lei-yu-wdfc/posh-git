using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.CardPayment.InternalMessages.SetServiceLoginPrimaryMessage </summary>
    [XmlRoot("SetServiceLoginPrimaryMessage", Namespace = "Wonga.CardPayment.InternalMessages", DataType = "")]
    public partial class SetServiceLoginPrimaryCommand : MsmqMessage<SetServiceLoginPrimaryCommand>
    {
        public Guid ServiceLoginId { get; set; }
    }
}
