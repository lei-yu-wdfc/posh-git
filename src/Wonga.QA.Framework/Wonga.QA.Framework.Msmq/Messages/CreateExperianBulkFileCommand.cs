using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.ExperianBulk.InternalMessages.CreateExperianBulkFileMessage </summary>
    [XmlRoot("CreateExperianBulkFileMessage", Namespace = "Wonga.ExperianBulk.InternalMessages", DataType = "")]
    public partial class CreateExperianBulkFileCommand : MsmqMessage<CreateExperianBulkFileCommand>
    {
    }
}
