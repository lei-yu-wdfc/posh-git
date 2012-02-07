using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("IAccountPrepared", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IRiskEvent")]
    public partial class IAccountPreparedEvent : MsmqMessage<IAccountPreparedEvent>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid PaymenCardId { get; set; }
        public Guid BankAccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
