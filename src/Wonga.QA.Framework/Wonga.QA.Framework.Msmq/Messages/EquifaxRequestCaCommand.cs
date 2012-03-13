using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.InternalMessages.Equifax.EquifaxRequestMessage </summary>
    [XmlRoot("EquifaxRequestMessage", Namespace = "Wonga.Risk.InternalMessages.Equifax", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class EquifaxRequestCaCommand : MsmqMessage<EquifaxRequestCaCommand>
    {
        public Guid AccountId { get; set; }
        public String EmployerName { get; set; }
        public String EmploymentPosition { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
