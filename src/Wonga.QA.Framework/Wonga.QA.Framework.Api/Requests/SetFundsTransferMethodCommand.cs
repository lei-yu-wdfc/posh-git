using System;
using System.Xml.Serialization;

namespace Wonga.QA.Framework.Api
{
    /// <summary> Wonga.Payments.Commands.SetFundsTransferMethodCommand </summary>
    [XmlRoot("SetFundsTransferMethodCommand")]
    public partial class SetFundsTransferMethodCommand : ApiRequest<SetFundsTransferMethodCommand>
    {
        public Object ApplicationId { get; set; }
        public Object TransferMethod { get; set; }
    }
}
