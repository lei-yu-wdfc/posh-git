using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Commands.Wb.Uk
{
    /// <summary> Wonga.Payments.Commands.Wb.Uk.AddBusinessPaymentCard </summary>
    [XmlRoot("AddBusinessPaymentCard", Namespace = "Wonga.Payments.Commands.Wb.Uk", DataType = "" )
    , SourceAssembly("Wonga.Payments.Commands.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class AddBusinessPaymentCard : MsmqMessage<AddBusinessPaymentCard>
    {
        public Guid OrganisationId { get; set; }
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
