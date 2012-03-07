using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.URU.CheckAgeUruResponseMessage </summary>
    [XmlRoot("CheckAgeUruResponseMessage", Namespace = "Wonga.Risk.URU", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow")]
    public partial class CheckAgeUruResponseUkCommand : MsmqMessage<CheckAgeUruResponseUkCommand>
    {
        public Boolean AgeUnder18 { get; set; }
        public Boolean AgeConfirmed { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
