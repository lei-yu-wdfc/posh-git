using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.ExperianBulk
{
    [XmlRoot("CreateExperianBulkFileMessage", Namespace = "Wonga.ExperianBulk.InternalMessages", DataType = "")]
    public partial class CreateExperianBulkFileCommand : MsmqMessage<CreateExperianBulkFileCommand>
    {
    }
}
