using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Commands.RevokeApplicationFromDca </summary>
    [XmlRoot("RevokeApplicationFromDca")]
    public partial class RevokeApplicationFromDcaCommand : CsRequest<RevokeApplicationFromDcaCommand>
    {
        public Object ApplicationId { get; set; }
    }
}
