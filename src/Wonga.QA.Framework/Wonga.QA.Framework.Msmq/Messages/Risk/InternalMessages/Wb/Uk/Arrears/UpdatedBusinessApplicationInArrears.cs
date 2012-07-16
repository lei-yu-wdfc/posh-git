using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.InternalMessages.Wb.Uk.Arrears
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.Arrears.UpdatedBusinessApplicationInArrears </summary>
    [XmlRoot("UpdatedBusinessApplicationInArrears", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk.Arrears", DataType = "")]
    public partial class UpdatedBusinessApplicationInArrears : MsmqMessage<UpdatedBusinessApplicationInArrears>
    {
        public Guid ApplicationId { get; set; }
        public Int32 DaysInArrears { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
