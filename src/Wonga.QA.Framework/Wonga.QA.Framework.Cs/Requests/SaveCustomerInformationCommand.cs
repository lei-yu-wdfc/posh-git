using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Comms.Csapi.Commands.SaveCustomerInformation </summary>
    [XmlRoot("SaveCustomerInformation")]
    public partial class SaveCustomerInformationCommand : CsRequest<SaveCustomerInformationCommand>
    {
        public Object AccountId { get; set; }
        public Object HomePhone { get; set; }
        public Object MobilePhone { get; set; }
        public Object WorkPhone { get; set; }
        public Object NationalNumber { get; set; }
        public Object PostCode { get; set; }
        public Object Town { get; set; }
        public Object PreferredLanguage { get; set; }
        public Object RequestType { get; set; }
    }
}
