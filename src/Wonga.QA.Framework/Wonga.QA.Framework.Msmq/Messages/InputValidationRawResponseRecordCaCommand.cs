using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq
{
    /// <summary> Wonga.BankGateway.InternalMessages.Scotiabank.Ca.InputValidationRawResponseRecordMessage </summary>
    [XmlRoot("InputValidationRawResponseRecordMessage", Namespace = "Wonga.BankGateway.InternalMessages.Scotiabank.Ca", DataType = "")]
    public partial class InputValidationRawResponseRecordCaCommand : MsmqMessage<InputValidationRawResponseRecordCaCommand>
    {
        public Object InputValidationRawResponseRecord { get; set; }
        public String Filename { get; set; }
        public Byte[] FileContents { get; set; }
    }
}
