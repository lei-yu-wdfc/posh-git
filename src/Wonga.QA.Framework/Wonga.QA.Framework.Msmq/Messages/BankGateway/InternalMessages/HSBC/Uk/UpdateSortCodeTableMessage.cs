using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.HSBC.Uk
{
    /// <summary> Wonga.BankGateway.InternalMessages.HSBC.Uk.UpdateSortCodeTableMessage </summary>
    [XmlRoot("UpdateSortCodeTableMessage", Namespace = "Wonga.BankGateway.InternalMessages.HSBC.Uk", DataType = "" )
    , SourceAssembly("Wonga.BankGateway.InternalMessages.HSBC.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class UpdateSortCodeTableMessage : MsmqMessage<UpdateSortCodeTableMessage>
    {
        public Byte ServiceId { get; set; }
    }
}
