using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.InternalMessages.Ca.BritishColombia.CreateBcPreApproveDirectDebitMessage </summary>
    [XmlRoot("CreateBcPreApproveDirectDebitMessage", Namespace = "Wonga.Comms.InternalMessages.Ca.BritishColombia", DataType = "")]
    public partial class CreateBcPreApproveDirectDebitCaCommand : MsmqMessage<CreateBcPreApproveDirectDebitCaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
