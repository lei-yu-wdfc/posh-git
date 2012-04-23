using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Commands.WriteOffApplication </summary>
    [XmlRoot("WriteOffApplication")]
    public partial class WriteOffApplicationCommand : CsRequest<WriteOffApplicationCommand>
    {
        public Object ApplicationId { get; set; }
        public Object DoNotRelend { get; set; }
    }
}
