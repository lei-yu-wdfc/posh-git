using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Messages
{
    /// <summary> Wonga.Comms.Messages.CreateLoanDocuments </summary>
    [XmlRoot("CreateLoanDocuments", Namespace = "Wonga.Comms.Messages", DataType = "" )
    , SourceAssembly("Wonga.Comms.Messages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreateLoanDocuments : MsmqMessage<CreateLoanDocuments>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
