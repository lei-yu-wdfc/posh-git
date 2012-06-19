using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Comms.Csapi.Commands.CsRemoveComplaint </summary>
    [XmlRoot("CsRemoveComplaint")]
    public partial class CsRemoveComplaintCommand : CsRequest<CsRemoveComplaintCommand>
    {
        public Object ApplicationId { get; set; }
        public Object AccountId { get; set; }
        public Object CaseId { get; set; }
    }
}
