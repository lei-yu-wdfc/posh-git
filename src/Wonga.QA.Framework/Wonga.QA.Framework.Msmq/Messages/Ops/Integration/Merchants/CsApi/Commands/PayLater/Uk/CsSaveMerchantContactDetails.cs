using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Comms.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Ops.Integration.Merchants.CsApi.Commands.PayLater.Uk
{
    /// <summary> Wonga.Ops.Integration.Merchants.CsApi.Commands.PayLater.Uk.CsSaveMerchantContactDetails </summary>
    [XmlRoot("CsSaveMerchantContactDetails", Namespace = "Wonga.Ops.Integration.Merchants.CsApi.Commands.PayLater.Uk", DataType = "" )
    , SourceAssembly("Wonga.Ops.Integration.Merchants.CsApi.Commands.PayLater.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CsSaveMerchantContactDetails : MsmqMessage<CsSaveMerchantContactDetails>
    {
        public Guid MerchantId { get; set; }
        public String ContactName { get; set; }
        public TitleEnum? Title { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
