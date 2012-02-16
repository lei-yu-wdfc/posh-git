using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.BankGateway
{
    [XmlRoot("UpdateSortCodeTableMessage", Namespace = "Wonga.BankGateway.InternalMessages", DataType = "")]
    public partial class UpdateSortCodeTableCommand : MsmqMessage<UpdateSortCodeTableCommand>
    {
    }
}
