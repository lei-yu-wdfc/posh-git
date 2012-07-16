using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Comms.Prepaid.Uk.Instructions
{
    /// <summary> Wonga.PublicMessages.Comms.Prepaid.Uk.Instructions.IWantToCreateSuccessfulStandartCardCreationEmail </summary>
    [XmlRoot("IWantToCreateSuccessfulStandartCardCreationEmail", Namespace = "Wonga.PublicMessages.Comms.Prepaid.Uk.Instructions", DataType = "")]
    public partial class IWantToCreateSuccessfulStandartCardCreationEmail : MsmqMessage<IWantToCreateSuccessfulStandartCardCreationEmail>
    {
        public Guid AccountId { get; set; }
    }
}
