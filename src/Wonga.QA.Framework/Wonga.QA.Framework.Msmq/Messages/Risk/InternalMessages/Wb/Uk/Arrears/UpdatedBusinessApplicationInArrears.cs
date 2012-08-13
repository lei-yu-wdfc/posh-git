using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Wb.Uk.Arrears
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.Arrears.UpdatedBusinessApplicationInArrears </summary>
    [XmlRoot("UpdatedBusinessApplicationInArrears", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk.Arrears", DataType = "" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Wb.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class UpdatedBusinessApplicationInArrears : MsmqMessage<UpdatedBusinessApplicationInArrears>
    {
        public Guid ApplicationId { get; set; }
        public Int32 DaysInArrears { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
