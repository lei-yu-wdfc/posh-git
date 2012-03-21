using System.Net;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class SubmitClientWatermarkCommand
    {
        public override void Default()
        {
            AccountId = Get.GetId();
            ApplicationId = Get.GetId();
            ClientIPAddress = "127.0.0.1";
            BlackboxData = "foobar";
        }
    }
}
