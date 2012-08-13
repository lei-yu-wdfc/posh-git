using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.AccountRankChanged </summary>
    [XmlRoot("AccountRankChanged", Namespace = "Wonga.Risk", DataType = "" )
    , SourceAssembly("Wonga.Risk.InternalMessages, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class AccountRankChanged : MsmqMessage<AccountRankChanged>
    {
        public Guid AccountId { get; set; }
        public Int32 Rank { get; set; }
    }
}
