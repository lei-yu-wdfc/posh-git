using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.UpdateSortCodeTableMessage </summary>
    [XmlRoot("UpdateSortCodeTableMessage", Namespace = "Wonga.BankGateway.InternalMessages", DataType = "")]
    public partial class UpdateSortCodeTableCommand : MsmqMessage<UpdateSortCodeTableCommand>
    {
        public Byte InstanceId { get; set; }
    }
}
