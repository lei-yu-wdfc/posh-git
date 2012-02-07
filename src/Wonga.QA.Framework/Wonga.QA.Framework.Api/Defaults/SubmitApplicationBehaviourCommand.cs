using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Api
{
    public partial class SubmitApplicationBehaviourCommand
    {
        public override void Default()
        {
            ApplicationId = Data.GetId();
            TermSliderPosition = Data.RandomEnum<SliderPositionEnum>();
            AmountSliderPosition = Data.RandomEnum<SliderPositionEnum>();
        }
    }
}
