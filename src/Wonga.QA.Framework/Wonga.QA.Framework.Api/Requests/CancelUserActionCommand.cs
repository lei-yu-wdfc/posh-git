using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("CancelUserAction")]
    public class CancelUserActionCommand : ApiRequest<CancelUserActionCommand>
    {
        public Object UserActionId { get; set; }
    }
}
