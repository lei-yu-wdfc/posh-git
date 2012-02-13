using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("CancelUserAction")]
    public partial class CancelUserActionCommand : ApiRequest<CancelUserActionCommand>
    {
        public Object UserActionId { get; set; }
    }
}
