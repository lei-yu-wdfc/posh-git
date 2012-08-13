using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Csapi.Commands
{
    /// <summary> Wonga.Comms.Csapi.Commands.CsRemoveComplaint </summary>
    [XmlRoot("CsRemoveComplaint", Namespace = "Wonga.Comms.Csapi.Commands", DataType = "" )
    , SourceAssembly("Wonga.Comms.Csapi.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CsRemoveComplaint : MsmqMessage<CsRemoveComplaint>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid CaseId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
