using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.InternalMessages.Ca.BritishColombia
{
    /// <summary> Wonga.Comms.InternalMessages.Ca.BritishColombia.CreateBcConfirmationOfPADAuthorisationMessage </summary>
    [XmlRoot("CreateBcConfirmationOfPADAuthorisationMessage", Namespace = "Wonga.Comms.InternalMessages.Ca.BritishColombia", DataType = "" )
    , SourceAssembly("Wonga.Comms.InternalMessages.Ca, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CreateBcConfirmationOfPADAuthorisationMessage : MsmqMessage<CreateBcConfirmationOfPADAuthorisationMessage>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
    }
}
