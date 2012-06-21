using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands
{
    /// <summary> Wonga.Risk.Commands.CancelUserAction </summary>
    [XmlRoot("CancelUserAction")]
    public partial class CancelUserActionCommand : ApiRequest<CancelUserActionCommand>
    {
        public Object UserActionId { get; set; }
    }
}
