using System;

namespace Wonga.QA.Framework.UI.Mappings.Pages
{
    internal sealed class AddressDetailsPage
    {
        internal String FormId { get; set; }
        internal String PostCode { get; set; }
        internal String FlatNumber { get; set; }
        internal String District { get; set; }
        internal String County { get; set; }
        internal String AddressPeriod { get; set; }
        internal String AddressOptions { get; set; }

        internal String LookupButton { get; set; }
        internal String NextButton { get; set; }
    }
}
