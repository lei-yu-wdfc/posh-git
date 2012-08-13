using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Za
{
    /// <summary> Wonga.Payments.Za.GenerateEasyPayNumber </summary>
    [XmlRoot("GenerateEasyPayNumber", Namespace = "Wonga.Payments.Za", DataType = "" )
    , SourceAssembly("Wonga.Payments.Commands.Za, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class GenerateEasyPayNumber : MsmqMessage<GenerateEasyPayNumber>
    {
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
