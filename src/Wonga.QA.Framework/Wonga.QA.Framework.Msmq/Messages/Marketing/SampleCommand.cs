using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Marketing
{
    [XmlRoot("SampleCommand", Namespace = "Wonga.Marketing.Commands", DataType = "")]
    public partial class SampleCommand : MsmqMessage<SampleCommand>
    {
        public String HelloWorld { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
