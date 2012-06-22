using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Messages.Ca.Ontario
{
    /// <summary> Wonga.Comms.Messages.Ca.Ontario.CreateLoanDocumentsForOn </summary>
    [XmlRoot("CreateLoanDocumentsForOn", Namespace = "Wonga.Comms.Messages.Ca.Ontario", DataType = "")]
    public partial class CreateLoanDocumentsForOnCaCommand : MsmqMessage<CreateLoanDocumentsForOnCaCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
