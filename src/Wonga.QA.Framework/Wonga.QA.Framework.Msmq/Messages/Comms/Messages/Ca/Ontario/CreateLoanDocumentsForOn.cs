using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Messages.Ca.Ontario
{
    /// <summary> Wonga.Comms.Messages.Ca.Ontario.CreateLoanDocumentsForOn </summary>
    [XmlRoot("CreateLoanDocumentsForOn", Namespace = "Wonga.Comms.Messages.Ca.Ontario", DataType = "" )
    , SourceAssembly("Wonga.Comms.Messages.Ca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreateLoanDocumentsForOn : MsmqMessage<CreateLoanDocumentsForOn>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
