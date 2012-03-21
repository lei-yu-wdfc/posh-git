using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
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
