using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Commands.CsReportBankruptcy </summary>
    [XmlRoot("CsReportBankruptcy")]
    public partial class CsReportBankruptcyCommand : CsRequest<CsReportBankruptcyCommand>
    {
        public Object CaseId { get; set; }
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
    }
}
