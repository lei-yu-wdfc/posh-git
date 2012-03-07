using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Bi.Messages.UpdateCollectionDate </summary>
    [XmlRoot("UpdateCollectionDate", Namespace = "Wonga.Bi.Messages", DataType = "")]
    public partial class UpdateCollectionDateCommand : MsmqMessage<UpdateCollectionDateCommand>
    {
        public Guid? ApplicationId { get; set; }
        public Guid AccountId { get; set; }
        public DateTime CollectionDate { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
