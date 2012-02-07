using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("ExperianBureauRequestMessage", Namespace = "Wonga.Risk.Experian", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class ExperianBureauRequestUkCommand : MsmqMessage<ExperianBureauRequestUkCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Object EmploymentDetails { get; set; }
        public Object PreviousAddress { get; set; }
        public Object SocialDetails { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
