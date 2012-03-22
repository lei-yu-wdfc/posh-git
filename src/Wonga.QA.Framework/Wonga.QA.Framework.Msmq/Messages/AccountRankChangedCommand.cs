using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.AccountRankChanged </summary>
    [XmlRoot("AccountRankChanged", Namespace = "Wonga.Risk", DataType = "")]
    public partial class AccountRankChangedCommand : MsmqMessage<AccountRankChangedCommand>
    {
        public Guid AccountId { get; set; }
        public Int32 Rank { get; set; }
    }
}
