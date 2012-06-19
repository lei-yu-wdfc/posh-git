using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Tests.Core;

namespace Wonga.QA.Tests.Ui.Mobile
{
    public class SlidersTests : UiMobileTest
    {
        [Test, AUT(AUT.Za)]
        public void IncreaseThenDecreaseAmountAndDurationValuesUsingSliders()
        {
            var homepage = Client.MobileHome();
            homepage.Sliders.HowMuch = "200";
            homepage.Sliders.HowLong = "10";
            //drag sliders increase values
            homepage.Sliders.MoveAmountSlider = 300;
            homepage.Sliders.MoveDurationSlider = 250;
            Assert.GreaterThan(Convert.ToInt32(homepage.Sliders.HowMuch), 200);
            Assert.GreaterThan(Convert.ToInt32(homepage.Sliders.HowLong), 10);
            var amtAfterSliderIncrease = homepage.Sliders.HowMuch;
            var durationAfterSliderIncrease = homepage.Sliders.HowLong;
            //drag sliders decrease values
            homepage.Sliders.MoveAmountSlider = -200;
            homepage.Sliders.MoveDurationSlider = -150;
            Assert.GreaterThan(Convert.ToInt32(amtAfterSliderIncrease), Convert.ToInt32(homepage.Sliders.HowMuch));
            Assert.GreaterThan(Convert.ToInt32(durationAfterSliderIncrease), Convert.ToInt32(homepage.Sliders.HowLong));
        }

        [Test, AUT(AUT.Za)]
        public void SlideAmountSlidersUntilMaximumValueIsReached()
        {
            var homepage = Client.MobileHome();
            homepage.Sliders.HowMuch = "0";
            //check that value defaults to 100 after 0 is entered in amount field
            Assert.AreEqual(homepage.Sliders.HowMuch, "100");
            //drag sliders until value is 2000
            do
            {
                Thread.Sleep(1000);
                homepage.Sliders.MoveAmountSlider = 90;
            } while (Convert.ToInt32(homepage.Sliders.HowMuch) < 2000);
        }
    }
}
