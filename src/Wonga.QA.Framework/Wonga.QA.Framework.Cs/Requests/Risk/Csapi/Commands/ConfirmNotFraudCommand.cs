using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Risk.Csapi.Commands
{
    /// <summary> Wonga.Risk.Csapi.Commands.ConfirmNotFraud </summary>
    [XmlRoot("ConfirmNotFraud")]
    public partial class ConfirmNotFraudCommand : CsRequest<ConfirmNotFraudCommand>
    {
        public Object AccountId { get; set; }
        public Object CaseId { get; set; }
    }
}
