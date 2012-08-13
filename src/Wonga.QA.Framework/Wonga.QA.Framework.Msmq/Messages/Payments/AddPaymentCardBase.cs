using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments
{
    /// <summary> Wonga.Payments.AddPaymentCardBase </summary>
    [XmlRoot("AddPaymentCardBase", Namespace = "Wonga.Payments", DataType = "" )
    , SourceAssembly("Wonga.Payments.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class AddPaymentCardBase : MsmqMessage<AddPaymentCardBase>
    {
        public Guid PaymentCardId { get; set; }
        public String CardType { get; set; }
        public String Number { get; set; }
        public String HolderName { get; set; }
        public String IssueNo { get; set; }
        public String SecurityCode { get; set; }
        public Boolean IsCreditCard { get; set; }
        public Boolean IsPrimary { get; set; }
        public Object AdditionalDetails { get; set; }
        public String StartDateXml { get; set; }
        public String ExpiryDateXml { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
