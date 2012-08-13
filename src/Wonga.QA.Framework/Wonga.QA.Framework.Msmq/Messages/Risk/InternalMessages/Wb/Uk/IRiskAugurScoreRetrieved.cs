using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Wb.Uk
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.IRiskAugurScoreRetrieved </summary>
    [XmlRoot("IRiskAugurScoreRetrieved", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk", DataType = "" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IRiskAugurScoreRetrieved : MsmqMessage<IRiskAugurScoreRetrieved>
    {
        public Guid ApplicationId { get; set; }
        public Int32 AugurScore { get; set; }
    }
}
