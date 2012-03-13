using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.Commands.ConfirmSmsPinMessage </summary>
    [XmlRoot("ConfirmSmsPinMessage", Namespace = "Wonga.Comms.Commands", DataType = "")]
    public partial class ConfirmSmsPinCommand : MsmqMessage<ConfirmSmsPinCommand>
    {
        public Guid AccountId { get; set; }
        public String MobilePhone { get; set; }
        public String Pin { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
