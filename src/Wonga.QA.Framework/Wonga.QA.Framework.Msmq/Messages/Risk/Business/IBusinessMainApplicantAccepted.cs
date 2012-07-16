using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Business
{
    /// <summary> Wonga.Risk.Business.IBusinessMainApplicantAccepted </summary>
    [XmlRoot("IBusinessMainApplicantAccepted", Namespace = "Wonga.Risk.Business", DataType = "Wonga.Risk.IRiskEvent")]
    public partial class IBusinessMainApplicantAccepted : MsmqMessage<IBusinessMainApplicantAccepted>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Guid OrganisationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
