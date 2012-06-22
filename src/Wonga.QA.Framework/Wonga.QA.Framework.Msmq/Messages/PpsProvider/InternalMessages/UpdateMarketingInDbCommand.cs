using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.UpdateMarketingInDBMessage </summary>
    [XmlRoot("UpdateMarketingInDBMessage", Namespace = "Wonga.PpsProvider.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class UpdateMarketingInDbCommand : MsmqMessage<UpdateMarketingInDbCommand>
    {
        public Guid SagaId { get; set; }
    }
}
