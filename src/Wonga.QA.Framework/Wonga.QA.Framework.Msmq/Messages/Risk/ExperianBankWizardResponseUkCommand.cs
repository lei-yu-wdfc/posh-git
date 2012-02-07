using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("ExperianBankWizardResponseMessage", Namespace = "Wonga.Risk.Experian", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow")]
    public partial class ExperianBankWizardResponseUkCommand : MsmqMessage<ExperianBankWizardResponseUkCommand>
    {
        public Object BWVerifyResponse { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
