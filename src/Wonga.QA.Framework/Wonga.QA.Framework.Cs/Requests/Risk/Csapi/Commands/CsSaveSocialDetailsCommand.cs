using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs.Requests.Risk.Csapi.Commands
{
    /// <summary> Wonga.Risk.Csapi.Commands.CsSaveSocialDetails </summary>
    [XmlRoot("CsSaveSocialDetails")]
    public partial class CsSaveSocialDetailsCommand : CsRequest<CsSaveSocialDetailsCommand>
    {
        public Object AccountId { get; set; }
        public Object MaritalStatus { get; set; }
        public Object OccupancyStatus { get; set; }
        public Object Dependants { get; set; }
    }
}
