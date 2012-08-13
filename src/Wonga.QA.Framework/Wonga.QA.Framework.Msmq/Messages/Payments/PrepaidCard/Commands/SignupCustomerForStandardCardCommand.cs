using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.PrepaidCard.Commands
{
    /// <summary> Wonga.Payments.PrepaidCard.Commands.SignupCustomerForStandardCardCommand </summary>
    [XmlRoot("SignupCustomerForStandardCardCommand", Namespace = "Wonga.Payments.PrepaidCard.Commands", DataType = "" )
    , SourceAssembly("Wonga.Payments.PrepaidCard.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SignupCustomerForStandardCardCommand : MsmqMessage<SignupCustomerForStandardCardCommand>
    {
        public Guid CustomerExternalId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
