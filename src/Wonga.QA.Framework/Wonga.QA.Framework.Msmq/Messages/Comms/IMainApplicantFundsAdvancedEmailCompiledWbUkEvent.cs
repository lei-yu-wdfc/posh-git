using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Comms
{
    /// <summary> Wonga.Comms.PublicMessages.Wb.Uk.IMainApplicantFundsAdvancedEmailCompiled </summary>
    [XmlRoot("IMainApplicantFundsAdvancedEmailCompiled", Namespace = "Wonga.Comms.PublicMessages.Wb.Uk", DataType = "")]
    public partial class IMainApplicantFundsAdvancedEmailCompiledWbUkEvent : MsmqMessage<IMainApplicantFundsAdvancedEmailCompiledWbUkEvent>
    {
        public Guid PrimaryDirectorAccountId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid DocumentId { get; set; }
    }
}
