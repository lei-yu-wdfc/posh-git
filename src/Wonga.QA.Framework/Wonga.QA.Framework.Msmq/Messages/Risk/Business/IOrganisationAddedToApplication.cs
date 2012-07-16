using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Business
{
    /// <summary> Wonga.Risk.Business.IOrganisationAddedToApplication </summary>
    [XmlRoot("IOrganisationAddedToApplication", Namespace = "Wonga.Risk.Business", DataType = "Wonga.Risk.IRiskEvent")]
    public partial class IOrganisationAddedToApplication : MsmqMessage<IOrganisationAddedToApplication>
    {
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
