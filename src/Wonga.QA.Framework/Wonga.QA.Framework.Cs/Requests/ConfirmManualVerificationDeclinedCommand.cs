using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Risk.Csapi.Commands.ConfirmManualVerificationDeclined </summary>
    [XmlRoot("ConfirmManualVerificationDeclined")]
    public partial class ConfirmManualVerificationDeclinedCommand : CsRequest<ConfirmManualVerificationDeclinedCommand>
    {
        public Object ApplicationId { get; set; }
    }
}
