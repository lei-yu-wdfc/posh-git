using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Experian
{
    /// <summary> Wonga.Risk.Experian.ExperianBankWizardRequestMessage </summary>
    [XmlRoot("ExperianBankWizardRequestMessage", Namespace = "Wonga.Risk.Experian", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class ExperianBankWizardRequestUkCommand : MsmqMessage<ExperianBankWizardRequestUkCommand>
    {
        public Guid AccountId { get; set; }
        public Guid BankAccountId { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
