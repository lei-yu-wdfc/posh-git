using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Bi.CustomerManagement.InternalMessages
{
    /// <summary> Wonga.Bi.CustomerManagement.InternalMessages.SaveApplicationStatusChange </summary>
    [XmlRoot("SaveApplicationStatusChange", Namespace = "Wonga.Bi.CustomerManagement.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.Bi.CustomerManagement.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SaveApplicationStatusChange : MsmqMessage<SaveApplicationStatusChange>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public String CurrentStatus { get; set; }
        public String PreviousStatus { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
