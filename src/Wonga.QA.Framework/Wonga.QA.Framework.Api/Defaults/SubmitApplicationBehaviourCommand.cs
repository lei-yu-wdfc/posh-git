using Wonga.QA.Framework.Api.Enums;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api.Requests.Risk.Commands
{
    public partial class SubmitApplicationBehaviourCommand
    {
        public override void Default()
        {
            ApplicationId = Get.GetId();
            TermSliderPosition = Get.RandomEnum<SliderPositionEnum>();
            AmountSliderPosition = Get.RandomEnum<SliderPositionEnum>();
        }
    }
}
