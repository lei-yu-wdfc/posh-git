using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("SetFundsTransferMethodCommand")]
    public partial class SetFundsTransferMethodCommand : ApiRequest<SetFundsTransferMethodCommand>
    {
        public Guid ApplicationId { get; set; }
        public int TransferMethod { get; set; }
    }
}
