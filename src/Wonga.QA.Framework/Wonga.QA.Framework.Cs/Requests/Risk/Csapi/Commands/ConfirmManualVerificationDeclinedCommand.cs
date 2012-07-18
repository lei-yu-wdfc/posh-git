using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Risk.Csapi.Commands
{
    /// <summary> Wonga.Risk.Csapi.Commands.ConfirmManualVerificationDeclined </summary>
    [XmlRoot("ConfirmManualVerificationDeclined")]
    public partial class ConfirmManualVerificationDeclinedCommand : CsRequest<ConfirmManualVerificationDeclinedCommand>
    {
        public Object ApplicationId { get; set; }
        public Object ProbabilityGood { get; set; }
    }
}
