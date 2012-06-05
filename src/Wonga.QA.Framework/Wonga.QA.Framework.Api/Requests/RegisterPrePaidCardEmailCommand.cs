using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Marketing.Commands.RegisterPrePaidCardEmailCommand </summary>
    [XmlRoot("RegisterPrePaidCardEmailCommand")]
    public partial class RegisterPrePaidCardEmailCommand : ApiRequest<RegisterPrePaidCardEmailCommand>
    {
        public Object Email { get; set; }
    }
}
