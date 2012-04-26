using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Risk.InternalMessages.Wb.Uk.Arrears.UpdatedBusinessApplicationInArrears </summary>
    [XmlRoot("UpdatedBusinessApplicationInArrears", Namespace = "Wonga.Risk.InternalMessages.Wb.Uk.Arrears", DataType = "")]
    public partial class UpdatedBusinessApplicationInArrearsWbUkCommand : MsmqMessage<UpdatedBusinessApplicationInArrearsWbUkCommand>
    {
        public Guid ApplicationId { get; set; }
        public Int32 DaysInArrears { get; set; }
        public Guid OrganisationId { get; set; }
    }
}
