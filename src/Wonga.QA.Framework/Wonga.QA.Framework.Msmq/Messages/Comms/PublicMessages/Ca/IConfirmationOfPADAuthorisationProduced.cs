using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.PublicMessages.Ca
{
    /// <summary> Wonga.Comms.PublicMessages.Ca.IConfirmationOfPADAuthorisationProduced </summary>
    [XmlRoot("IConfirmationOfPADAuthorisationProduced", Namespace = "Wonga.Comms.PublicMessages.Ca", DataType = "")]
    public partial class IConfirmationOfPADAuthorisationProduced : MsmqMessage<IConfirmationOfPADAuthorisationProduced>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid FileStorageId { get; set; }
    }
}
