using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.ApplicationArrearsStartDateSet </summary>
    [XmlRoot("ApplicationArrearsStartDateSet", Namespace = "Wonga.Risk", DataType = "")]
    public partial class ApplicationArrearsStartDateSetCommand : MsmqMessage<ApplicationArrearsStartDateSetCommand>
    {
        public Guid ApplicationId { get; set; }
        public DateTime StartDate { get; set; }
    }
}
