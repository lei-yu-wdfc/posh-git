using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Risk;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Commands.CA
{
    /// <summary> Wonga.Risk.Commands.CA.SaveSocialDetailsMessage </summary>
    [XmlRoot("SaveSocialDetailsMessage", Namespace = "Wonga.Risk.Commands.CA", DataType = "" )
    , SourceAssembly("Wonga.Risk.Commands.CA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SaveSocialDetails : MsmqMessage<SaveSocialDetails>
    {
        public String VehicleRegistration { get; set; }
        public Guid AccountId { get; set; }
        public OccupancyStatusEnum OccupancyStatus { get; set; }
        public Int32 Dependants { get; set; }
        public MaritalStatusEnum MaritalStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
