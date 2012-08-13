using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.CalculateCreditLimitMessage </summary>
    [XmlRoot("CalculateCreditLimitMessage", Namespace = "Wonga.Risk", DataType = "" )
    , SourceAssembly("Wonga.Risk.InternalMessages, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CalculateCreditLimitMessage : MsmqMessage<CalculateCreditLimitMessage>
    {
        public Guid AccountId { get; set; }
        public Guid? ApplicationId { get; set; }
    }
}
