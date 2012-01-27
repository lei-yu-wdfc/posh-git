using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Bi
{
    [XmlRoot("UpdateDailyAccruedInterest", Namespace = "Wonga.Bi.Messages", DataType = "")]
    public class UpdateDailyAccruedInterestCommand : MsmqMessage<UpdateDailyAccruedInterestCommand>
    {
        public Guid ApplicationId { get; set; }
        public DateTime AccountingDate { get; set; }
        public Decimal AccruedInterest { get; set; }
        public Decimal Balance { get; set; }
        public Decimal Charges { get; set; }
        public Decimal ExtendLoanPartPayment { get; set; }
        public Decimal PostedInterest { get; set; }
    }
}
