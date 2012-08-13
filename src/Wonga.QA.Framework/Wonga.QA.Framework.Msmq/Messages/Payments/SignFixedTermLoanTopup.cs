using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments
{
    /// <summary> Wonga.Payments.SignFixedTermLoanTopup </summary>
    [XmlRoot("SignFixedTermLoanTopup", Namespace = "Wonga.Payments", DataType = "" )
    , SourceAssembly("Wonga.Payments.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SignFixedTermLoanTopup : MsmqMessage<SignFixedTermLoanTopup>
    {
        public Guid AccountId { get; set; }
        public Guid FixedTermLoanTopupId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
