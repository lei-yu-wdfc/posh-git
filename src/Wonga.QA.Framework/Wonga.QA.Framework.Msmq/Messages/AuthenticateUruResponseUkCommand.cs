using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.Uru.AuthenticateUruResponseMessage </summary>
    [XmlRoot("AuthenticateUruResponseMessage", Namespace = "Wonga.Risk.Uru", DataType = "Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage,Wonga.Risk.IResumeRiskWorkflow")]
    public partial class AuthenticateUruResponseUkCommand : MsmqMessage<AuthenticateUruResponseUkCommand>
    {
        public Boolean IdentityMatched { get; set; }
        public Boolean ElectricMpanMatched { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
