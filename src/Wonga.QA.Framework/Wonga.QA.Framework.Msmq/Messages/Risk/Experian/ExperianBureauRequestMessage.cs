using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Experian
{
    /// <summary> Wonga.Risk.Experian.ExperianBureauRequestMessage </summary>
    [XmlRoot("ExperianBureauRequestMessage", Namespace = "Wonga.Risk.Experian", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ExperianBureauRequestMessage : MsmqMessage<ExperianBureauRequestMessage>
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
