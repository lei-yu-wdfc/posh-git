using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Ops
{
    [XmlRoot("SavePasswordRecoveryDetailsMessage", Namespace = "Wonga.Ops", DataType = "")]
    public partial class SavePasswordRecoveryDetailsCommand : MsmqMessage<SavePasswordRecoveryDetailsCommand>
    {
        public Guid AccountId { get; set; }
        public String SecretQuestion { get; set; }
        public String SecretAnswer { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
