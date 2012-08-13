using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Ops.Integration.Merchants.CsApi.Commands.PayLater.Uk
{
    /// <summary> Wonga.Ops.Integration.Merchants.CsApi.Commands.PayLater.Uk.CsCreateMerchantAccount </summary>
    [XmlRoot("CsCreateMerchantAccount", Namespace = "Wonga.Ops.Integration.Merchants.CsApi.Commands.PayLater.Uk", DataType = "" )
    , SourceAssembly("Wonga.Ops.Integration.Merchants.CsApi.Commands.PayLater.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CsCreateMerchantAccount : MsmqMessage<CsCreateMerchantAccount>
    {
        public Guid MerchantId { get; set; }
        public String Name { get; set; }
        public String PhysicalAddress { get; set; }
        public Int32 CategoryCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
