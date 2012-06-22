using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Risk;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.IEmploymentDetailsUpdated </summary>
    [XmlRoot("IEmploymentDetailsUpdated", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IRiskEvent")]
    public partial class IEmploymentDetailsUpdatedEvent : MsmqMessage<IEmploymentDetailsUpdatedEvent>
    {
        public Guid AccountId { get; set; }
        public IncomeFrequencyEnum? IncomeFrequency { get; set; }
        public DateTime? NextPayDate { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
