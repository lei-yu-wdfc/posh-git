using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Wb.Uk.Arrears
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.Arrears.BusinessApplicationArrearsStartDateSet </summary>
    [XmlRoot("BusinessApplicationArrearsStartDateSet", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk.Arrears", DataType = "" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class BusinessApplicationArrearsStartDateSet : MsmqMessage<BusinessApplicationArrearsStartDateSet>
    {
        public Guid ApplicationId { get; set; }
        public DateTime StartDate { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
