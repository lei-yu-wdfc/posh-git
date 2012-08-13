using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Uru
{
    /// <summary> Wonga.Risk.Uru.AuthenticateUruResponseMessage </summary>
    [XmlRoot("AuthenticateUruResponseMessage", Namespace = "Wonga.Risk.Uru", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class AuthenticateUruResponseMessage : MsmqMessage<AuthenticateUruResponseMessage>
    {
        public Boolean IdentityMatched { get; set; }
        public Boolean ElectricMpanMatched { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
