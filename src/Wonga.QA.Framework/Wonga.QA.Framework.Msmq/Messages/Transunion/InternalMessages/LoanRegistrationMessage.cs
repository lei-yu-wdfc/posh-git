using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Transunion.InternalMessages
{
    /// <summary> Wonga.Transunion.InternalMessages.LoanRegistrationMessage </summary>
    [XmlRoot("LoanRegistrationMessage", Namespace = "Wonga.Transunion.InternalMessages", DataType = "NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Transunion.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class LoanRegistrationMessage : MsmqMessage<LoanRegistrationMessage>
    {
        public Object BureauEnquiry { get; set; }
        public Guid SagaId { get; set; }
    }
}
