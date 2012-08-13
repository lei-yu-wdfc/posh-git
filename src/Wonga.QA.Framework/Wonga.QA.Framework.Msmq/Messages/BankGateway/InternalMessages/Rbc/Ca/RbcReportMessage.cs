using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.Rbc.Ca
{
    /// <summary> Wonga.BankGateway.InternalMessages.Rbc.Ca.RbcReportMessage </summary>
    [XmlRoot("RbcReportMessage", Namespace = "Wonga.BankGateway.InternalMessages.Rbc.Ca", DataType = "" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages.Rbc.Ca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class RbcReportMessage : MsmqMessage<RbcReportMessage>
    {
        public String FileName { get; set; }
        public Byte[] FileContent { get; set; }
    }
}
