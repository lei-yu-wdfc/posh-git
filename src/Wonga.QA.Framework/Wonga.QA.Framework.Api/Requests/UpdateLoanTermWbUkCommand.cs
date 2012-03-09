using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Commands.Wb.Uk.UpdateLoanTerm </summary>
    [XmlRoot("UpdateLoanTerm")]
    public partial class UpdateLoanTermWbUkCommand : ApiRequest<UpdateLoanTermWbUkCommand>
    {
        public Object ApplicationId { get; set; }
        public Object Term { get; set; }
    }
}
