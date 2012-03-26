using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.PublicMessages.Wb.Uk.ISecondaryDirectorFundsAdvancedEmailCompiled </summary>
    [XmlRoot("ISecondaryDirectorFundsAdvancedEmailCompiled", Namespace = "Wonga.Comms.PublicMessages.Wb.Uk", DataType = "")]
    public partial class ISecondaryDirectorFundsAdvancedEmailCompiledWbUkEvent : MsmqMessage<ISecondaryDirectorFundsAdvancedEmailCompiledWbUkEvent>
    {
        public Guid AccountId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid DocumentId { get; set; }
    }
}
