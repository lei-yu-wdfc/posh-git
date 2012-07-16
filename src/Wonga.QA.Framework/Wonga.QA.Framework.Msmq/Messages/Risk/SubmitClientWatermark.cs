using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.SubmitClientWatermarkCommand </summary>
    [XmlRoot("SubmitClientWatermarkCommand", Namespace = "Wonga.Risk", DataType = "")]
    public partial class SubmitClientWatermark : MsmqMessage<SubmitClientWatermark>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public String ClientIPAddress { get; set; }
        public String BlackboxData { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
