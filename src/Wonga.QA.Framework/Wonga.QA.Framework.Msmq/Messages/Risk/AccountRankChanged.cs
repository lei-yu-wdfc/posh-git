using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.AccountRankChanged </summary>
    [XmlRoot("AccountRankChanged", Namespace = "Wonga.Risk", DataType = "")]
    public partial class AccountRankChanged : MsmqMessage<AccountRankChanged>
    {
        public Guid AccountId { get; set; }
        public Int32 Rank { get; set; }
    }
}
