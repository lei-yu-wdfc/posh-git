using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands
{
    /// <summary> Wonga.Comms.Commands.SaveReviewDetailsMessage </summary>
    [XmlRoot("SaveReviewDetailsMessage", Namespace = "Wonga.Comms.Commands", DataType = "" )
    , SourceAssembly("Wonga.Comms.Commands, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SaveReviewDetails : MsmqMessage<SaveReviewDetails>
    {
        public Guid AccountId { get; set; }
        public Boolean DataIsReviewed { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
