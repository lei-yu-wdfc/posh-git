using System;
namespace Wonga.QA.Framework.UI.Mappings.Elements
{
    /// <summary>
    /// The Topup sliders Section
    /// </summary>
    public sealed class TopupSlidersElement
    {
        public String FormId { get; set; }
        public String LoanAmount { get; set; }
        public String SubmitButton { get; set; }
        public String TotalAmount { get; set; }
        public String TotalFees { get; set; }
        public String TotalToRepay { get; set; }
        public String AmountMinusButton { get; set; }
        public String AmountPlusButton { get; set; }
    }
}
