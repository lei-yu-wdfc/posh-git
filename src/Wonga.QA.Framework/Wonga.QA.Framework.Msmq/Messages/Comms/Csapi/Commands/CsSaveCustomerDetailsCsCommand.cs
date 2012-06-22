using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Comms.Enums;
using Wonga.QA.Framework.Msmq.Enums.Common.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.Comms.Csapi.Commands
{
    /// <summary> Wonga.Comms.Csapi.Commands.SaveCustomerDetailsCsApiMessage </summary>
    [XmlRoot("SaveCustomerDetailsCsApiMessage", Namespace = "Wonga.Comms.Csapi.Commands", DataType = "")]
    public partial class CsSaveCustomerDetailsCsCommand : MsmqMessage<CsSaveCustomerDetailsCsCommand>
    {
        public Guid AccountId { get; set; }
        public TitleEnum? Title { get; set; }
        public GenderEnum Gender { get; set; }
        public String Forename { get; set; }
        public String Surname { get; set; }
        public String Middlename { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String HomePhone { get; set; }
        public String MobilePhone { get; set; }
        public String WorkPhone { get; set; }
        public String NationalNumber { get; set; }
        public String PreferredLanguage { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
