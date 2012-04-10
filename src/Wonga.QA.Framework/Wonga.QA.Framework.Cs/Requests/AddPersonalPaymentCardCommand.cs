using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Commands.AddPersonalPaymentCard </summary>
    [XmlRoot("AddPersonalPaymentCard")]
    public partial class AddPersonalPaymentCardCommand : CsRequest<AddPersonalPaymentCardCommand>
    {
        public Object AccountId { get; set; }
        public Object CardType { get; set; }
        public Object Number { get; set; }
        public Object HolderName { get; set; }
        public Object StartDate { get; set; }
        public Object ExpiryDate { get; set; }
        public Object IssueNo { get; set; }
        public Object SecurityCode { get; set; }
        public Object IsCreditCard { get; set; }
        public Object IsPrimary { get; set; }
    }
}
