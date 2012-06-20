using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Wb.Uk
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.IRiskAugurScoreRetrieved </summary>
    [XmlRoot("IRiskAugurScoreRetrieved", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk", DataType = "")]
    public partial class IRiskAugurScoreRetrievedWbUkEvent : MsmqMessage<IRiskAugurScoreRetrievedWbUkEvent>
    {
        public Guid ApplicationId { get; set; }
        public Int32 AugurScore { get; set; }
    }
}
