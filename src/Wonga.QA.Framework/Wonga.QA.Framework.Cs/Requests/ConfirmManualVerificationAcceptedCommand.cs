using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Risk.Csapi.Commands.ConfirmManualVerificationAccepted </summary>
    [XmlRoot("ConfirmManualVerificationAccepted")]
    public partial class ConfirmManualVerificationAcceptedCommand : CsRequest<ConfirmManualVerificationAcceptedCommand>
    {
        public Object ApplicationId { get; set; }
    }
}
