using System;
namespace Wonga.QA.Framework.UI.Mappings.Elements
{
    /// <summary>
    /// The sliders Section
    /// </summary>
    public sealed class SmallTopupSlidersElement
    {
        public String FormId { get; set; }
        public String AmountSlider { get; set; }
        public String TopupLoanAmount { get; set; }
        public String SubmitButton { get; set; }
        public String TopupAmount { get; set; }
        public String TopupFees { get; set; }
        public String TotalToRepay { get; set; }
        public String TopupToRepay { get; set; }
        public String RepaymentDate { get; set; }
        public String AmountMinusButton { get; set; }
        public String AmountPlusButton { get; set; }
    }
}
