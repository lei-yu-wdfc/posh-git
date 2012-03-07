using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.Comms.DocumentGeneration.InternalMessages.Za.SavePlainEmailFileMessage </summary>
    [XmlRoot("SavePlainEmailFileMessage", Namespace = "Wonga.Comms.DocumentGeneration.InternalMessages.Za", DataType = "")]
    public partial class SavePlainEmailFileZaCommand : MsmqMessage<SavePlainEmailFileZaCommand>
    {
        public Guid OriginatingSagaId { get; set; }
        public Byte[] Content { get; set; }
    }
}
