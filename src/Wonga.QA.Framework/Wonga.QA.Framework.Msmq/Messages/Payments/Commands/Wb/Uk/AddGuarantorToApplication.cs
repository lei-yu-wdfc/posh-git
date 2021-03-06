using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Commands.Wb.Uk
{
    /// <summary> Wonga.Payments.Commands.Wb.Uk.AddGuarantorToApplication </summary>
    [XmlRoot("AddGuarantorToApplication", Namespace = "Wonga.Payments.Commands.Wb.Uk", DataType = "" )
    , SourceAssembly("Wonga.Payments.Commands.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class AddGuarantorToApplication : MsmqMessage<AddGuarantorToApplication>
    {
        public Guid ApplicationId { get; set; }
        public Guid GuarantorId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
