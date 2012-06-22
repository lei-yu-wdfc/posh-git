using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Common.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Commands.PL
{
    /// <summary> Wonga.Risk.Commands.PL.RiskSaveCustomerDetailsMessage </summary>
    [XmlRoot("RiskSaveCustomerDetailsMessage", Namespace = "Wonga.Risk.Commands.PL", DataType = "")]
    public partial class RiskSaveCustomerDetailsCommand : MsmqMessage<RiskSaveCustomerDetailsCommand>
    {
        public String PeselNumber { get; set; }
        public String IdentificationDocumentId { get; set; }
        public Guid AccountId { get; set; }
        public GenderEnum Gender { get; set; }
        public String Forename { get; set; }
        public String Surname { get; set; }
        public String MiddleName { get; set; }
        public String HomePhone { get; set; }
        public String WorkPhone { get; set; }
        public String MobilePhone { get; set; }
        public String Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String MaidenName { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
