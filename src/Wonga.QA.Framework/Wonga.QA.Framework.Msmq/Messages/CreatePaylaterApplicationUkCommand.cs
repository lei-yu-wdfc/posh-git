using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Payments.Commands.PLater.Uk.CreatePayLaterApplication </summary>
    [XmlRoot("CreatePayLaterApplication", Namespace = "Wonga.Payments.Commands.PLater.Uk", DataType = "")]
    public partial class CreatePaylaterApplicationUkCommand : MsmqMessage<CreatePaylaterApplicationUkCommand>
    {
        public Guid AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public Guid MerchantId { get; set; }
        public Decimal TotalAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
