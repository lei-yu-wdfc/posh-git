using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Events.Wb.Uk
{
    /// <summary> Wonga.Comms.InternalMessages.Events.Wb.Uk.IApplicationRatesAddedTranslated </summary>
    [XmlRoot("IApplicationRatesAddedTranslated", Namespace = "Wonga.Comms.InternalMessages.Events.Wb.Uk", DataType = "Wonga.Comms.PublicMessages.ICommsEvent" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Events.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IApplicationRatesAddedTranslated : MsmqMessage<IApplicationRatesAddedTranslated>
    {
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
