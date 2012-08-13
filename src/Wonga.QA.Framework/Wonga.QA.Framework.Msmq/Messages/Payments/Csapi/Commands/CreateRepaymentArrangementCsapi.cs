using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Csapi.Commands
{
    /// <summary> Wonga.Payments.Csapi.Commands.CreateRepaymentArrangementCsapi </summary>
    [XmlRoot("CreateRepaymentArrangementCsapi", Namespace = "Wonga.Payments.Csapi.Commands", DataType = "" )
    , SourceAssembly("Wonga.Payments.Csapi.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreateRepaymentArrangementCsapi : MsmqMessage<CreateRepaymentArrangementCsapi>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Decimal EffectiveBalance { get; set; }
        public Decimal RepaymentAmount { get; set; }
        public Object ArrangementDetails { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
