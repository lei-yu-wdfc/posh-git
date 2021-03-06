using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Salesforce.InternalMessages.Za
{
    /// <summary> Wonga.Salesforce.InternalMessages.Za.EasyPayNumberAddedMessage </summary>
    [XmlRoot("EasyPayNumberAddedMessage", Namespace = "Wonga.Salesforce.InternalMessages.Za", DataType = "" )
    , SourceAssembly("Wonga.Salesforce.InternalMessages.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class EasyPayNumberAddedMessage : MsmqMessage<EasyPayNumberAddedMessage>
    {
        public Guid AccountId { get; set; }
    }
}
