using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Commands.FlagApplicationToDca </summary>
    [XmlRoot("FlagApplicationToDca")]
    public partial class FlagApplicationToDcaCommand : CsRequest<FlagApplicationToDcaCommand>
    {
        public Object ApplicationId { get; set; }
    }
}
