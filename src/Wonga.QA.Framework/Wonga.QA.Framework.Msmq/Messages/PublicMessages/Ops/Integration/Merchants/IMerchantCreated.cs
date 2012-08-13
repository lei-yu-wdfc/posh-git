using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.PublicMessages.Ops.Integration.Merchants
{
    /// <summary> Wonga.PublicMessages.Ops.Integration.Merchants.IMerchantCreated </summary>
    [XmlRoot("IMerchantCreated", Namespace = "Wonga.PublicMessages.Ops.Integration.Merchants", DataType = "" )
    , SourceAssembly("Wonga.PublicMessages.Ops.Integration.Merchants, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IMerchantCreated : MsmqMessage<IMerchantCreated>
    {
        public Guid MerchantId { get; set; }
    }
}
