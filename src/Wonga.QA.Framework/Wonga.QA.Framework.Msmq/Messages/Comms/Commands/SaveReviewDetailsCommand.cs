using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands
{
    /// <summary> Wonga.Comms.Commands.SaveReviewDetailsMessage </summary>
    [XmlRoot("SaveReviewDetailsMessage", Namespace = "Wonga.Comms.Commands", DataType = "")]
    public partial class SaveReviewDetailsCommand : MsmqMessage<SaveReviewDetailsCommand>
    {
        public Guid AccountId { get; set; }
        public Boolean DataIsReviewed { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
