using System.ComponentModel;
namespace Wonga.QA.Framework.Api
{
    public enum FundsTransferMethodEnum
    {
        [Description("DefaultAutomaticallyChosen")]
        DefaultAutomaticallyChosen,
        [Description("SendToPrepaidCard")]
        SendToPrepaidCard,
    }
}
