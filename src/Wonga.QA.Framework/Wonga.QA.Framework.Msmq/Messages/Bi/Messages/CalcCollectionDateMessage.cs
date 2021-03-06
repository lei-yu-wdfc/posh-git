using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Bi.Messages
{
    /// <summary> Wonga.Bi.Messages.CalcCollectionDateMessage </summary>
    [XmlRoot("CalcCollectionDateMessage", Namespace = "Wonga.Bi.Messages", DataType = "" )
    , SourceAssembly("Wonga.Bi.Messages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CalcCollectionDateMessage : MsmqMessage<CalcCollectionDateMessage>
    {
        public Guid AccountId { get; set; }
        public Guid? ApplicationId { get; set; }
        public DateTime PromiseDate { get; set; }
        public DateTime? NextDueDate { get; set; }
    }
}
