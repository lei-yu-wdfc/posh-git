using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.IssueCardByProviderMessage </summary>
    [XmlRoot("IssueCardByProviderMessage", Namespace = "Wonga.PpsProvider.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IssueCardByProviderCommand : MsmqMessage<IssueCardByProviderCommand>
    {
        public Guid SagaId { get; set; }
    }
}
