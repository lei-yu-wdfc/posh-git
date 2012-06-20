using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Payments.Csapi.Commands
{
    /// <summary> Wonga.Payments.Csapi.Commands.DeletePersonalPaymentCard </summary>
    [XmlRoot("DeletePersonalPaymentCard", Namespace = "Wonga.Payments.Csapi.Commands", DataType = "")]
    public partial class DeletePersonalPaymentCardCsCommand : MsmqMessage<DeletePersonalPaymentCardCsCommand>
    {
        public Guid PaymentCardId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
