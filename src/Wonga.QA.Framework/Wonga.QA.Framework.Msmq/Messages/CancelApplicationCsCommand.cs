using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.Csapi.Commands.AbandonApplication </summary>
    [XmlRoot("AbandonApplication", Namespace = "Wonga.Payments.Csapi.Commands", DataType = "")]
    public partial class CancelApplicationCsCommand : MsmqMessage<CancelApplicationCsCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
