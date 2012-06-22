using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk
{
    /// <summary> Wonga.Risk.UpdatedApplicationInArrears </summary>
    [XmlRoot("UpdatedApplicationInArrears", Namespace = "Wonga.Risk", DataType = "")]
    public partial class UpdatedApplicationInArrearsCommand : MsmqMessage<UpdatedApplicationInArrearsCommand>
    {
        public Guid ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public Int32 DaysInArrears { get; set; }
    }
}
