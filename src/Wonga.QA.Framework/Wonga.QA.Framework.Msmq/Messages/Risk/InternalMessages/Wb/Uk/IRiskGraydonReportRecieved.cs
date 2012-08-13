using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Wb.Uk
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.IRiskGraydonReportRecieved </summary>
    [XmlRoot("IRiskGraydonReportRecieved", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk", DataType = "" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class IRiskGraydonReportRecieved : MsmqMessage<IRiskGraydonReportRecieved>
    {
        public Guid ApplicationId { get; set; }
        public Guid OrganisationId { get; set; }
        public Object Report { get; set; }
    }
}
