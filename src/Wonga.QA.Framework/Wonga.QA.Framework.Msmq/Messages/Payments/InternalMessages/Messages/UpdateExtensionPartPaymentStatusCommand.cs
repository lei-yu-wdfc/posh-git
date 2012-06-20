using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Payments.InternalMessages.Messages
{
    /// <summary> Wonga.Payments.InternalMessages.Messages.UpdateExtensionPartPaymentStatus </summary>
    [XmlRoot("UpdateExtensionPartPaymentStatus", Namespace = "Wonga.Payments.InternalMessages.Messages", DataType = "")]
    public partial class UpdateExtensionPartPaymentStatusCommand : MsmqMessage<UpdateExtensionPartPaymentStatusCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid ExtensionId { get; set; }
        public Boolean IsSuccess { get; set; }
    }
}
