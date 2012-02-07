using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    [XmlRoot("SendPaymentConfirmationEmailMessage", Namespace = "Wonga.Comms.InternalMessages.Email.Za", DataType = "Wonga.Comms.InternalMessages.Email.SendPaymentConfirmationEmailMessage,Wonga.Comms.InternalMessages.SagaMessages.BaseSagaMessage,NServiceBus.Saga.ISagaMessage")]
    public partial class SendPaymentConfirmationEmailZaCommand : MsmqMessage<SendPaymentConfirmationEmailZaCommand>
    {
        public Guid AccountId { get; set; }
        public String Email { get; set; }
        public String Forename { get; set; }
        public String LoanAmount { get; set; }
        public String TotalRepayable { get; set; }
        public String PromiseDate { get; set; }
        public String CustomerReference { get; set; }
        public Guid SagaId { get; set; }
    }
}
