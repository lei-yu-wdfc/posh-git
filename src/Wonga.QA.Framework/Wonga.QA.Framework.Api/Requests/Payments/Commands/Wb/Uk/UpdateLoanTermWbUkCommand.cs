using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api.Requests.Payments.Commands.Wb.Uk
{
    /// <summary> Wonga.Payments.Commands.Wb.Uk.UpdateLoanTerm </summary>
    [XmlRoot("UpdateLoanTerm")]
    public partial class UpdateLoanTermWbUkCommand : ApiRequest<UpdateLoanTermWbUkCommand>
    {
        public Object ApplicationId { get; set; }
        public Object Term { get; set; }
    }
}
