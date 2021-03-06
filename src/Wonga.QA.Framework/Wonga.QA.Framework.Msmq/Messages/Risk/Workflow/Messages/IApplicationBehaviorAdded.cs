using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Workflow.Messages
{
    /// <summary> Wonga.Risk.Workflow.Messages.IApplicationBehaviorAdded </summary>
    [XmlRoot("IApplicationBehaviorAdded", Namespace = "Wonga.Risk.Workflow.Messages", DataType = "" )
    , SourceAssembly("Wonga.Risk.Workflow.Messages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IApplicationBehaviorAdded : MsmqMessage<IApplicationBehaviorAdded>
    {
        public Guid ApplicationId { get; set; }
    }
}
