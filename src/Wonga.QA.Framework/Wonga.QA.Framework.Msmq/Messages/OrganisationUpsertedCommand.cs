using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Salesforce.InternalMessages.OrganisationUpserted </summary>
    [XmlRoot("OrganisationUpserted", Namespace = "Wonga.Salesforce.InternalMessages", DataType = "")]
    public partial class OrganisationUpsertedCommand : MsmqMessage<OrganisationUpsertedCommand>
    {
        public Guid OrganisationId { get; set; }
    }
}
