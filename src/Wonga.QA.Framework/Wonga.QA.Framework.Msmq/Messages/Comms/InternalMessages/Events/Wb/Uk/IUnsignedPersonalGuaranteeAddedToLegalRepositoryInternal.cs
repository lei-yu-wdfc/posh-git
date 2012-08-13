using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Events.Wb.Uk
{
    /// <summary> Wonga.Comms.InternalMessages.Events.Wb.Uk.IUnsignedPersonalGuaranteeAddedToLegalRepositoryInternal </summary>
    [XmlRoot("IUnsignedPersonalGuaranteeAddedToLegalRepositoryInternal", Namespace = "Wonga.Comms.InternalMessages.Events.Wb.Uk", DataType = "Wonga.Comms.PublicMessages.Wb.Uk.IUnsignedPersonalGuaranteeAddedToLegalRepository,Wonga.Comms.PublicMessages.Wb.Uk.IPersonalGuaranteeAddedToLegalRepository" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Events.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IUnsignedPersonalGuaranteeAddedToLegalRepositoryInternal : MsmqMessage<IUnsignedPersonalGuaranteeAddedToLegalRepositoryInternal>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid ExternalId { get; set; }
    }
}
