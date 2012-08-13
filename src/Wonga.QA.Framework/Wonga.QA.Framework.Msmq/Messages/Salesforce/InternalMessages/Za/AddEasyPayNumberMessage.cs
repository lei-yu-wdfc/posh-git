using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Salesforce.InternalMessages.Za
{
    /// <summary> Wonga.Salesforce.InternalMessages.Za.AddEasyPayNumberMessage </summary>
    [XmlRoot("AddEasyPayNumberMessage", Namespace = "Wonga.Salesforce.InternalMessages.Za", DataType = "" )
    , SourceAssembly("Wonga.Salesforce.InternalMessages.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class AddEasyPayNumberMessage : MsmqMessage<AddEasyPayNumberMessage>
    {
        public Guid AccountId { get; set; }
        public String EasyPayNumber { get; set; }
    }
}
