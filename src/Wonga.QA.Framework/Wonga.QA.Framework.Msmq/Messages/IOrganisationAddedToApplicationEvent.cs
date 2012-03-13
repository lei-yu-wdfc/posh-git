using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.Business.IOrganisationAddedToApplication </summary>
    [XmlRoot("IOrganisationAddedToApplication", Namespace = "Wonga.Risk.Business", DataType = "Wonga.Risk.IRiskEvent")]
    public partial class IOrganisationAddedToApplicationEvent : MsmqMessage<IOrganisationAddedToApplicationEvent>
    {
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
