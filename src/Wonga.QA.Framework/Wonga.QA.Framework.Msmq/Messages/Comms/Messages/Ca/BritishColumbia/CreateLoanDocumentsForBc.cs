using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Messages.Ca.BritishColumbia
{
    /// <summary> Wonga.Comms.Messages.Ca.BritishColumbia.CreateLoanDocumentsForBc </summary>
    [XmlRoot("CreateLoanDocumentsForBc", Namespace = "Wonga.Comms.Messages.Ca.BritishColumbia", DataType = "")]
    public partial class CreateLoanDocumentsForBc : MsmqMessage<CreateLoanDocumentsForBc>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
