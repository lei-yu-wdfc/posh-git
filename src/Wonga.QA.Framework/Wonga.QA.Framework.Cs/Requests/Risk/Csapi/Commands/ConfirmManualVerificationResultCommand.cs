using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Risk.Csapi.Commands
{
    /// <summary> Wonga.Risk.Csapi.Commands.ConfirmManualVerificationResult </summary>
    [XmlRoot("ConfirmManualVerificationResult")]
    public partial class ConfirmManualVerificationResultCommand : CsRequest<ConfirmManualVerificationResultCommand>
    {
        public Object ApplicationId { get; set; }
        public Object ProbabilityGood { get; set; }
    }
}
