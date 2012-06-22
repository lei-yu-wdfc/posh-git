using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Messages.Ca.Alberta
{
    /// <summary> Wonga.Comms.Messages.Ca.Alberta.CreateLoanDocumentsForAb </summary>
    [XmlRoot("CreateLoanDocumentsForAb", Namespace = "Wonga.Comms.Messages.Ca.Alberta", DataType = "")]
    public partial class CreateLoanDocumentsForAbCaCommand : MsmqMessage<CreateLoanDocumentsForAbCaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
