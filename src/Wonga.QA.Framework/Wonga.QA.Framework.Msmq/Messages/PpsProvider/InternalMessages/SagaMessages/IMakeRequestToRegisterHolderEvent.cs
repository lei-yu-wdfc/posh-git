using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.PrepaidCard.DataEntity;
using Wonga.QA.Framework.Msmq.Enums.Integration.Comms.Enums;
using Wonga.QA.Framework.Msmq.Enums.Common.Enums;

namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages.SagaMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.SagaMessages.IMakeRequestToRegisterHolderMessage </summary>
    [XmlRoot("IMakeRequestToRegisterHolderMessage", Namespace = "Wonga.PpsProvider.InternalMessages.SagaMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class IMakeRequestToRegisterHolderEvent : MsmqMessage<IMakeRequestToRegisterHolderEvent>
    {
        public ProcessStatusEnum HolderStatus { get; set; }
        public Guid ExternalId { get; set; }
        public String Flat { get; set; }
        public String HouseNumber { get; set; }
        public String HouseName { get; set; }
        public String Street { get; set; }
        public String District { get; set; }
        public String Town { get; set; }
        public String County { get; set; }
        public String Postcode { get; set; }
        public CountryCodeEnum CountryCode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public GenderEnum Gender { get; set; }
        public TitleEnum? Title { get; set; }
        public String Forename { get; set; }
        public String Surname { get; set; }
        public String HomePhoneAreaCode { get; set; }
        public String HomePhoneLocalNumber { get; set; }
        public String MobilePhoneAreaCode { get; set; }
        public String MobilePhoneLocalNumber { get; set; }
        public String Email { get; set; }
        public Guid SagaId { get; set; }
    }
}
