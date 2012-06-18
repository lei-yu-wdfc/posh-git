using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.Mobile.Mappings.Elements
{
    /// <summary>
    /// The sliders Section
    /// </summary>
    public sealed class SlidersElement
    {
        public String FormId { get; set; }
        public String AmountSlider { get; set; }
        public String DurationSlider { get; set; }
        public String LoanAmount { get; set; }
        public String LoanDuration { get; set; }
        public String SubmitButton { get; set; }
        public String TotalAmount { get; set; }
        public String TotalFees { get; set; }
        public String TotalToRepay { get; set; }
        public String RepaymentDate { get; set; }
        public String AmountMinusButton { get; set; }
        public String AmountPlusButton { get; set; }
        public String DurationMinusButton { get; set; }
        public String DurationPlusButton { get; set; }
    }
}
