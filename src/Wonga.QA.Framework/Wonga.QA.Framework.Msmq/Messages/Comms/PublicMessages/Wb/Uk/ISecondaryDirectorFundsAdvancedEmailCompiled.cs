using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages.Wb.Uk
{
    /// <summary> Wonga.Comms.PublicMessages.Wb.Uk.ISecondaryDirectorFundsAdvancedEmailCompiled </summary>
    [XmlRoot("ISecondaryDirectorFundsAdvancedEmailCompiled", Namespace = "Wonga.Comms.PublicMessages.Wb.Uk", DataType = "")]
    public partial class ISecondaryDirectorFundsAdvancedEmailCompiled : MsmqMessage<ISecondaryDirectorFundsAdvancedEmailCompiled>
    {
        public Guid AccountId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid DocumentId { get; set; }
    }
}
