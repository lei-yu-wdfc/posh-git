using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Csapi.Commands
{
    /// <summary> Wonga.Comms.Csapi.Commands.CsRemoveManagementReview </summary>
    [XmlRoot("CsRemoveManagementReview", Namespace = "Wonga.Comms.Csapi.Commands", DataType = "")]
    public partial class CsRemoveManagementReviewCsCommand : MsmqMessage<CsRemoveManagementReviewCsCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid CaseId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
