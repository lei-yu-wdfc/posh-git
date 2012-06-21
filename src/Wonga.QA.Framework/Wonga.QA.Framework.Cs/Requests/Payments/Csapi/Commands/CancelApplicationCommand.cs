using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Payments.Csapi.Commands
{
    /// <summary> Wonga.Payments.Csapi.Commands.CancelApplication </summary>
    [XmlRoot("CancelApplication")]
    public partial class CancelApplicationCommand : CsRequest<CancelApplicationCommand>
    {
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
    }
}
