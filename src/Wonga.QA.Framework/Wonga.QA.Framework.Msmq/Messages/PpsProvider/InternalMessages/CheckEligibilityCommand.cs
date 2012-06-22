using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.CheckEligibilityMessage </summary>
    [XmlRoot("CheckEligibilityMessage", Namespace = "Wonga.PpsProvider.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class CheckEligibilityCommand : MsmqMessage<CheckEligibilityCommand>
    {
        public Guid SagaId { get; set; }
    }
}
