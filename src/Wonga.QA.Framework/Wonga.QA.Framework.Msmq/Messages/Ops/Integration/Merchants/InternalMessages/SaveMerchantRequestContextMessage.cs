using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Ops.Integration.Merchants.InternalMessages
{
    /// <summary> Wonga.Ops.Integration.Merchants.InternalMessages.SaveMerchantRequestContextMessage </summary>
    [XmlRoot("SaveMerchantRequestContextMessage", Namespace = "Wonga.Ops.Integration.Merchants.InternalMessages", DataType = "" )
    , SourceAssembly("Wonga.Ops.Integration.Merchants.InternalMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SaveMerchantRequestContextMessage : MsmqMessage<SaveMerchantRequestContextMessage>
    {
        public Object MerchantRequestContext { get; set; }
    }
}
