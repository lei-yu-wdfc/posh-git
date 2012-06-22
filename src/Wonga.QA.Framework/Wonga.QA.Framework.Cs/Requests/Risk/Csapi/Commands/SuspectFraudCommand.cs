using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Risk.Csapi.Commands
{
    /// <summary> Wonga.Risk.Csapi.Commands.SuspectFraud </summary>
    [XmlRoot("SuspectFraud")]
    public partial class SuspectFraudCommand : CsRequest<SuspectFraudCommand>
    {
        public Object AccountId { get; set; }
        public Object CaseId { get; set; }
    }
}
