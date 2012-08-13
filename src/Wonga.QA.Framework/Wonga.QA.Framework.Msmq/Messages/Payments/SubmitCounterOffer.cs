using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments
{
    /// <summary> Wonga.Payments.SubmitCounterOffer </summary>
    [XmlRoot("SubmitCounterOffer", Namespace = "Wonga.Payments", DataType = "" )
    , SourceAssembly("Wonga.Payments.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SubmitCounterOffer : MsmqMessage<SubmitCounterOffer>
    {
        public Guid ApplicationId { get; set; }
        public Guid UserActionId { get; set; }
        public Decimal NewLoanAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
