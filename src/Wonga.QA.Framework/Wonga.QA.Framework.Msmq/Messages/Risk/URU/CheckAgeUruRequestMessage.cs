using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.URU
{
    /// <summary> Wonga.Risk.URU.CheckAgeUruRequestMessage </summary>
    [XmlRoot("CheckAgeUruRequestMessage", Namespace = "Wonga.Risk.URU", DataType = "Wonga.Risk.Uru.UruRequestMessage,Wonga.Risk.BaseSagaMessage,NServiceBus.Saga.ISagaMessage" )
    , SourceAssembly("Wonga.Risk.InternalMessages.Uk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class CheckAgeUruRequestMessage : MsmqMessage<CheckAgeUruRequestMessage>
    {
        public Guid AccountId { get; set; }
        public String IpAddress { get; set; }
        public String MpanNumber1 { get; set; }
        public String MpanNumber2 { get; set; }
        public String MpanNumber3 { get; set; }
        public String MpanNumber4 { get; set; }
        public String ElectricMailSort { get; set; }
        public Guid SagaId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
