using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Payments.PrepaidCard.Csapi.Commands
{
    /// <summary> Wonga.Payments.PrepaidCard.Csapi.Commands.CsAdjustPrepaidCardAccountBalance </summary>
    [XmlRoot("CsAdjustPrepaidCardAccountBalance")]
    public partial class CsAdjustPrepaidCardAccountBalanceCommand : CsRequest<CsAdjustPrepaidCardAccountBalanceCommand>
    {
        public Object Amount { get; set; }
        public Object Currency { get; set; }
        public Object BalanceAdjustmentlId { get; set; }
        public Object PrepaidCardId { get; set; }
        public Object Reason { get; set; }
        public Object SalesForceUser { get; set; }
    }
}
