using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands
{
    /// <summary> Wonga.Payments.Csapi.Commands.TakePaymentByHierarchy </summary>
    [XmlRoot("TakePaymentByHierarchy")]
    public partial class TakePaymentByHierarchyCommand : CsRequest<TakePaymentByHierarchyCommand>
    {
        public Object CV2 { get; set; }
        public Object PaymentCardId { get; set; }
        public Object AccountId { get; set; }
        public Object Amount { get; set; }
        public Object Currency { get; set; }
        public Object SalesforceUser { get; set; }
    }
}
