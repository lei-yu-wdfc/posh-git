using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Comms.Commands.Za.UpdateCustomerGenderZa </summary>
    [XmlRoot("UpdateCustomerGenderZa")]
    public partial class UpdateCustomerGenderZaCommand : ApiRequest<UpdateCustomerGenderZaCommand>
    {
        public Object AccountId { get; set; }
        public Object Gender { get; set; }
    }
}
