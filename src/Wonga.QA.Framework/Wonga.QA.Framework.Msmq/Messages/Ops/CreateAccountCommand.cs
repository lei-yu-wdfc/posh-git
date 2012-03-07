using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Ops
{
    /// <summary> Wonga.Ops.CreateAccountMessage </summary>
    [XmlRoot("CreateAccountMessage", Namespace = "Wonga.Ops", DataType = "")]
    public partial class CreateAccountCommand : MsmqMessage<CreateAccountCommand>
    {
        public Guid AccountId { get; set; }
        public String Login { get; set; }
        public String Password { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
