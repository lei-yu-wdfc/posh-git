using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.ApplicationArrearsStartDateSet </summary>
    [XmlRoot("ApplicationArrearsStartDateSet", Namespace = "Wonga.Risk", DataType = "" )
    , SourceAssembly("Wonga.Risk.InternalMessages, Version=3.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class ApplicationArrearsStartDateSet : MsmqMessage<ApplicationArrearsStartDateSet>
    {
        public Guid ApplicationId { get; set; }
        public DateTime StartDate { get; set; }
    }
}
