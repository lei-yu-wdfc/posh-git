using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.Csapi.Commands.WriteOffApplication </summary>
    [XmlRoot("WriteOffApplication", Namespace = "Wonga.Payments.Csapi.Commands", DataType = "")]
    public partial class WriteOffApplicationCsCommand : MsmqMessage<WriteOffApplicationCsCommand>
    {
        public Guid ApplicationId { get; set; }
        public Boolean DoNotRelend { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
