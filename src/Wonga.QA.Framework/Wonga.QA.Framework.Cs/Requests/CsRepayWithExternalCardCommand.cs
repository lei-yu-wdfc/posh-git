using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Commands.CsRepayWithExternalCard </summary>
    [XmlRoot("CsRepayWithExternalCard")]
    public partial class CsRepayWithExternalCardCommand : CsRequest<CsRepayWithExternalCardCommand>
    {
        public Object AccountId { get; set; }
        public Object HolderName { get; set; }
        public Object CardType { get; set; }
        public Object CardNumber { get; set; }
        public Object CV2 { get; set; }
        public Object StartDate { get; set; }
        public Object ExpiryDate { get; set; }
        public Object IssueNo { get; set; }
        public Object Amount { get; set; }
        public Object Currency { get; set; }
        public Object AddressLine1 { get; set; }
        public Object AddressLine2 { get; set; }
        public Object Town { get; set; }
        public Object County { get; set; }
        public Object PostCode { get; set; }
        public Object Country { get; set; }
        public Object SalesforceUser { get; set; }
    }
}
