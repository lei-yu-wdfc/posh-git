using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.ContactManagement.InternalMessages
{
    /// <summary> Wonga.Comms.ContactManagement.InternalMessages.UpdateOrganisationVatNumberMessage </summary>
    [XmlRoot("UpdateOrganisationVatNumberMessage", Namespace = "Wonga.Comms.ContactManagement.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.Comms.ContactManagement.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class UpdateOrganisationVatNumberMessage : MsmqMessage<UpdateOrganisationVatNumberMessage>
    {
        public Guid OrganisationId { get; set; }
        public String VatNumber { get; set; }
    }
}
