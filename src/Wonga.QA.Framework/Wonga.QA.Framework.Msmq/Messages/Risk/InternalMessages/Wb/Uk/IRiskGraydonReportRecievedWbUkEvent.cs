using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Wb.Uk
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.IRiskGraydonReportRecieved </summary>
    [XmlRoot("IRiskGraydonReportRecieved", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk", DataType = "")]
    public partial class IRiskGraydonReportRecievedWbUkEvent : MsmqMessage<IRiskGraydonReportRecievedWbUkEvent>
    {
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public Object Report { get; set; }
    }
}
