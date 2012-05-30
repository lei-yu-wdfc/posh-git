using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.Za.SavePayUResponse </summary>
    [XmlRoot("SavePayUResponse", Namespace = "Wonga.Payments.Za", DataType = "")]
    public partial class SavePayUResponseZaCommand : MsmqMessage<SavePayUResponseZaCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid MerchantReferenceNumber { get; set; }
        public Decimal TransactionAmount { get; set; }
        public String RawRequestResponse { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
