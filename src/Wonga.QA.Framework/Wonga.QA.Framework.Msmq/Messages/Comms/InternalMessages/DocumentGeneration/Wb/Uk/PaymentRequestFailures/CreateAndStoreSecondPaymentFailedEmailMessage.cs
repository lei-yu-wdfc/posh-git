using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.DocumentGeneration.Wb.Uk.PaymentRequestFailures
{
    /// <summary> Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.PaymentRequestFailures.CreateAndStoreSecondPaymentFailedEmailMessage </summary>
    [XmlRoot("CreateAndStoreSecondPaymentFailedEmailMessage", Namespace = "Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk.PaymentRequestFailures", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages.DocumentGeneration.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreateAndStoreSecondPaymentFailedEmailMessage : MsmqMessage<CreateAndStoreSecondPaymentFailedEmailMessage>
    {
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
