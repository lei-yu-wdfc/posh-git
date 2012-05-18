using System;
using System.Xml.Serialization;
using Wonga.QA.Framework.Api.Enums;

namespace Wonga.QA.Framework.Api
{
    [XmlRoot("SetFundsTransferMethodCommand")]
    public partial class SetFundsTransferMethodCommand : ApiRequest<SetFundsTransferMethodCommand>
    {
        public Guid ApplicationId { get; set; }
        public FundsTransferEnum TransferMethod { get; set; }
    }
}
