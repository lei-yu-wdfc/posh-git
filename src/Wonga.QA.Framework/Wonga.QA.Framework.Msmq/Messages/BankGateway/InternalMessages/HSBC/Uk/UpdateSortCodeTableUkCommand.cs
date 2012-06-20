using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.BankGateway.InternalMessages.HSBC.Uk
{
    /// <summary> Wonga.BankGateway.InternalMessages.HSBC.Uk.UpdateSortCodeTableMessage </summary>
    [XmlRoot("UpdateSortCodeTableMessage", Namespace = "Wonga.BankGateway.InternalMessages.HSBC.Uk", DataType = "")]
    public partial class UpdateSortCodeTableUkCommand : MsmqMessage<UpdateSortCodeTableUkCommand>
    {
        public Byte ServiceId { get; set; }
    }
}
