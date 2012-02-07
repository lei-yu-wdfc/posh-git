using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Risk
{
    [XmlRoot("NeedIovationResultMessage", Namespace = "Wonga.Risk.Iovation", DataType = "")]
    public partial class NeedIovationResultCommand : MsmqMessage<NeedIovationResultCommand>
    {
    }
}
