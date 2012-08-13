using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using Wonga.QA.Framework.Msmq.Enums.Integration.Risk;

namespace Wonga.QA.Framework.Msmq.Messages.Risk.Commands.PL
{
    /// <summary> Wonga.Risk.Commands.PL.SaveEducationalBackgroundDetailsMessage </summary>
    [XmlRoot("SaveEducationalBackgroundDetailsMessage", Namespace = "Wonga.Risk.Commands.PL", DataType = "" )
    , SourceAssembly("Wonga.Risk.Commands.PL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null")]
    public partial class SaveEducationalBackgroundDetails : MsmqMessage<SaveEducationalBackgroundDetails>
    {
        public Guid AccountId { get; set; }
        public EducationalLevelEnum EducationalLevel { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
