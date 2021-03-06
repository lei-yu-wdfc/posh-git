using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Risk.Csapi.Commands
{
    /// <summary> Wonga.Risk.Csapi.Commands.ConfirmFraud </summary>
    [XmlRoot("ConfirmFraud")]
    public partial class ConfirmFraudCommand : CsRequest<ConfirmFraudCommand>
    {
        public Object AccountId { get; set; }
        public Object CaseId { get; set; }
    }
}
