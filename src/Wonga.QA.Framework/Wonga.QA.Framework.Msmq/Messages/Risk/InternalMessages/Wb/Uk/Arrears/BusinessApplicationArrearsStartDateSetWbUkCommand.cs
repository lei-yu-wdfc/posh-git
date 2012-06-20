using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Wb.Uk.Arrears
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.Arrears.BusinessApplicationArrearsStartDateSet </summary>
    [XmlRoot("BusinessApplicationArrearsStartDateSet", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk.Arrears", DataType = "")]
    public partial class BusinessApplicationArrearsStartDateSetWbUkCommand : MsmqMessage<BusinessApplicationArrearsStartDateSetWbUkCommand>
    {
        public Guid ApplicationId { get; set; }
        public DateTime StartDate { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
