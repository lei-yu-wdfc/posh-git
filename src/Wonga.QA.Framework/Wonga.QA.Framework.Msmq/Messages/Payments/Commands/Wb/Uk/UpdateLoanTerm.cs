using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Msmq.Messages.Payments.Commands.Wb.Uk
{
    /// <summary> Wonga.Payments.Commands.Wb.Uk.UpdateLoanTerm </summary>
    [XmlRoot("UpdateLoanTerm", Namespace = "Wonga.Payments.Commands.Wb.Uk", DataType = "")]
    public partial class UpdateLoanTerm : MsmqMessage<UpdateLoanTerm>
    {
        public Guid ApplicationId { get; set; }
        public Int32 Term { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ClientId { get; set; }
    }
}
