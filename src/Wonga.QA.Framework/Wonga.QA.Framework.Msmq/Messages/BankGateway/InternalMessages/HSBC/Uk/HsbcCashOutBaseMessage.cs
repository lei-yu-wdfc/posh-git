using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.HSBC.Uk
{
    /// <summary> Wonga.BankGateway.InternalMessages.HSBC.Uk.HsbcCashOutBaseMessage </summary>
    [XmlRoot("HsbcCashOutBaseMessage", Namespace = "Wonga.BankGateway.InternalMessages.HSBC.Uk", DataType = "" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages.HSBC.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class HsbcCashOutBaseMessage : MsmqMessage<HsbcCashOutBaseMessage>
    {
        public Byte ServiceId { get; set; }
    }
}
