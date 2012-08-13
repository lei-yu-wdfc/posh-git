using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Business
{
    /// <summary> Wonga.Risk.Business.IBusinessBankAccountAddedToApplication </summary>
    [XmlRoot("IBusinessBankAccountAddedToApplication", Namespace = "Wonga.Risk.Business", DataType = "" )
    , SourceAssembly("Wonga.Risk.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IBusinessBankAccountAddedToApplication : MsmqMessage<IBusinessBankAccountAddedToApplication>
    {
        public Guid OrganisationId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid BusinessBankAccountId { get; set; }
    }
}
