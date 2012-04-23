using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Commands.CancelApplication </summary>
    [XmlRoot("CancelApplication")]
    public partial class CancelApplicationCommand : CsRequest<CancelApplicationCommand>
    {
        public Object AccountId { get; set; }
        public Object ApplicationId { get; set; }
    }
}
