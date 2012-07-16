using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PublicMessages
{
    /// <summary> Wonga.Payments.PublicMessages.IFundsTransferToDefaultChosen </summary>
    [XmlRoot("IFundsTransferToDefaultChosen", Namespace = "Wonga.Payments.PublicMessages", DataType = "Wonga.Payments.PublicMessages.IPaymentsEvent")]
    public partial class IFundsTransferToDefaultChosen : MsmqMessage<IFundsTransferToDefaultChosen>
    {
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
