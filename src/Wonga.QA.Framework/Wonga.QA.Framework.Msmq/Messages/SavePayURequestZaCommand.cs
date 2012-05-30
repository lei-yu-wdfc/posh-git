using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.Za.SavePayURequest </summary>
    [XmlRoot("SavePayURequest", Namespace = "Wonga.Payments.Za", DataType = "")]
    public partial class SavePayURequestZaCommand : MsmqMessage<SavePayURequestZaCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid MerchantReferenceNumber { get; set; }
        public Decimal TransactionAmount { get; set; }
        public DateTime RequestedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
