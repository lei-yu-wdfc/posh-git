using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.Csapi.Commands.FlagApplicationToDca </summary>
    [XmlRoot("FlagApplicationToDca", Namespace = "Wonga.Payments.Csapi.Commands", DataType = "")]
    public partial class FlagApplicationToDcaCsCommand : MsmqMessage<FlagApplicationToDcaCsCommand>
    {
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
