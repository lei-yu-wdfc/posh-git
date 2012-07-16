using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Csapi.Commands
{
    /// <summary> Wonga.Payments.Csapi.Commands.CsExtendFixedTermLoan </summary>
    [XmlRoot("CsExtendFixedTermLoan", Namespace = "Wonga.Payments.Csapi.Commands", DataType = "")]
    public partial class CsExtendFixedTermLoan : MsmqMessage<CsExtendFixedTermLoan>
    {
        public Guid ApplicationId { get; set; }
        public Guid PaymentCardId { get; set; }
        public Guid LoanExtensionId { get; set; }
        public String CV2 { get; set; }
        public Decimal PartPaymentAmount { get; set; }
        public String SalesForceUser { get; set; }
        public DateTime ExtensionDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
