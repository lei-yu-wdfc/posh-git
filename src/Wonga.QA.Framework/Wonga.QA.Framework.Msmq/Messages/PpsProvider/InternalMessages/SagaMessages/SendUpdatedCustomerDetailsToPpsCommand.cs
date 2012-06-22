using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Comms.Enums;
using Wonga.QA.Framework.Msmq.Enums.Common.Enums;
using Wonga.QA.Framework.Msmq.Enums.Integration.PrepaidCard.DataEntity;

namespace Wonga.QA.Framework.Msmq.Messages.PpsProvider.InternalMessages.SagaMessages
{
    /// <summary> Wonga.PpsProvider.InternalMessages.SagaMessages.SendUpdatedCustomerDetailsToPpsMessage </summary>
    [XmlRoot("SendUpdatedCustomerDetailsToPpsMessage", Namespace = "Wonga.PpsProvider.InternalMessages.SagaMessages", DataType = "NServiceBus.Saga.ISagaMessage")]
    public partial class SendUpdatedCustomerDetailsToPpsCommand : MsmqMessage<SendUpdatedCustomerDetailsToPpsCommand>
    {
        public Guid SagaId { get; set; }
        public String ProviderId { get; set; }
        public TitleEnum? Title { get; set; }
        public String HomeAreaCode { get; set; }
        public String HomeLocalNumber { get; set; }
        public String MobileAreaCode { get; set; }
        public String MobileLocalNumber { get; set; }
        public String WorkAreaCode { get; set; }
        public String WorkLocalNumber { get; set; }
        public Guid AccountId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String Email { get; set; }
        public String Forename { get; set; }
        public GenderEnum Gender { get; set; }
        public Int32 CustomerDetailsId { get; set; }
        public String MaidenName { get; set; }
        public String MiddleName { get; set; }
        public String NationalNumber { get; set; }
        public String Surname { get; set; }
        public Guid AddressId { get; set; }
        public DateTime AtAddressFrom { get; set; }
        public DateTime? AtAddressTo { get; set; }
        public CountryCodeEnum CountryCode { get; set; }
        public String County { get; set; }
        public String District { get; set; }
        public String Flat { get; set; }
        public String HouseName { get; set; }
        public String HouseNumber { get; set; }
        public String Postcode { get; set; }
        public String Street { get; set; }
        public String Town { get; set; }
        public ProcessStatusEnum HolderStatus { get; set; }
        public Guid HolderExternalId { get; set; }
        public Int32 HolderId { get; set; }
    }
}
