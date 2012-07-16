using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Csapi.Commands
{
    /// <summary> Wonga.Payments.Csapi.Commands.FlagApplicationToDca </summary>
    [XmlRoot("FlagApplicationToDca", Namespace = "Wonga.Payments.Csapi.Commands", DataType = "")]
    public partial class FlagApplicationToDca : MsmqMessage<FlagApplicationToDca>
    {
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
