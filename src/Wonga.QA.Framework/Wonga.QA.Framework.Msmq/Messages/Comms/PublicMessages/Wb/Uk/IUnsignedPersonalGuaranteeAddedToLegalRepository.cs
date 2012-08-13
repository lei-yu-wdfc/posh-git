using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages.Wb.Uk
{
    /// <summary> Wonga.Comms.PublicMessages.Wb.Uk.IUnsignedPersonalGuaranteeAddedToLegalRepository </summary>
    [XmlRoot("IUnsignedPersonalGuaranteeAddedToLegalRepository", Namespace = "Wonga.Comms.PublicMessages.Wb.Uk", DataType = "Wonga.Comms.PublicMessages.Wb.Uk.IPersonalGuaranteeAddedToLegalRepository" )
    , SourceAssembly("Wonga.Comms.PublicMessages.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IUnsignedPersonalGuaranteeAddedToLegalRepository : MsmqMessage<IUnsignedPersonalGuaranteeAddedToLegalRepository>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid ExternalId { get; set; }
    }
}
