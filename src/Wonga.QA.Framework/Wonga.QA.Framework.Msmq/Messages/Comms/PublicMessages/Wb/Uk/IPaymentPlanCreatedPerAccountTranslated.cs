using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages.Wb.Uk
{
    /// <summary> Wonga.Comms.PublicMessages.Wb.Uk.IPaymentPlanCreatedPerAccountTranslated </summary>
    [XmlRoot("IPaymentPlanCreatedPerAccountTranslated", Namespace = "Wonga.Comms.PublicMessages.Wb.Uk", DataType = "Wonga.Comms.PublicMessages.ICommsEvent" )
    , SourceAssembly("Wonga.Comms.PublicMessages.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IPaymentPlanCreatedPerAccountTranslated : MsmqMessage<IPaymentPlanCreatedPerAccountTranslated>
    {
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
