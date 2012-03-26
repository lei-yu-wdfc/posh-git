using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Cs
{
    /// <summary> Wonga.Payments.Csapi.Commands.CreateTransaction </summary>
    [XmlRoot("CreateTransaction")]
    public partial class CreateTransactionCommand : CsRequest<CreateTransactionCommand>
    {
        public Object ApplicationGuid { get; set; }
        public Object Scope { get; set; }
        public Object Type { get; set; }
        public Object Amount { get; set; }
        public Object Currency { get; set; }
        public Object Reference { get; set; }
        public Object SalesForceUser { get; set; }
    }
}
