using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Risk;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.IEmploymentDetailsUpdated </summary>
    [XmlRoot("IEmploymentDetailsUpdated", Namespace = "Wonga.Risk", DataType = "Wonga.Risk.IRiskEvent" )
    , SourceAssembly("Wonga.Risk.PublicMessages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IEmploymentDetailsUpdated : MsmqMessage<IEmploymentDetailsUpdated>
    {
        public Guid AccountId { get; set; }
        public IncomeFrequencyEnum? IncomeFrequency { get; set; }
        public DateTime? NextPayDate { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
