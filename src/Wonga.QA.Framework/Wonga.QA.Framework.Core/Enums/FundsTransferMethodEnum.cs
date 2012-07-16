using System.ComponentModel;
namespace Wonga.QA.Framework.Api.Enums
{
    public enum FundsTransferMethodEnum
    {
        [Description("DefaultAutomaticallyChosen")]
        DefaultAutomaticallyChosen,
        [Description("SendToPrepaidCard")]
        SendToPrepaidCard,
    }
}
