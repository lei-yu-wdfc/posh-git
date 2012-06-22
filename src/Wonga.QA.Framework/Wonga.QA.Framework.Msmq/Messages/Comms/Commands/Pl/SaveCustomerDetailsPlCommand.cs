using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Comms.Enums;
using Wonga.QA.Framework.Msmq.Enums.Common.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Commands.Pl
{
    /// <summary> Wonga.Comms.Commands.Pl.SaveCustomerDetailsPlMessage </summary>
    [XmlRoot("SaveCustomerDetailsPlMessage", Namespace = "Wonga.Comms.Commands.Pl", DataType = "")]
    public partial class SaveCustomerDetailsPlCommand : MsmqMessage<SaveCustomerDetailsPlCommand>
    {
        public String PeselNumber { get; set; }
        public Guid AccountId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public TitleEnum? Title { get; set; }
        public GenderEnum Gender { get; set; }
        public String Forename { get; set; }
        public String Surname { get; set; }
        public String MiddleName { get; set; }
        public String HomePhone { get; set; }
        public String WorkPhone { get; set; }
        public String MobilePhone { get; set; }
        public String Email { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
