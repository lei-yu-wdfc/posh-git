using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Api
{
    /// <summary> Wonga.Api.Command </summary>
    [XmlRoot("Command", Namespace = "Wonga.Api", DataType = "" )
    , SourceAssembly("Wonga.Api, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class Command : MsmqMessage<Command>
    {
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
