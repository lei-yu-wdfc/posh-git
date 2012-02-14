using Wonga.QA.Framework.UI.Mappings;

namespace Wonga.QA.Framework.UI.Mappings
{
    internal abstract class BaseElements
    {
        internal virtual Sections.YourNameElement YourNameElement { get; set; }
        internal virtual Sections.YourDetailsElement YourDetailsElement { get; set; }
        internal virtual Sections.MobilePinVerificationElement MobilePinVerificationElement { get; set; }
        internal virtual Sections.ContactingYouElement ContactingYouElement { get; set; }
        internal virtual Sections.SliderElement SliderElement { get; set; }
        internal virtual Sections.AccountDetailsElement AccountDetailsElement { get; set; }
        internal virtual Sections.DebitCardElement DebitCardElement { get; set; }
        internal virtual Sections.BankAccountElement BankAccountElement { get; set; }

        #region WbPages

        internal virtual Pages.Wb.EligibilityQuestionsPage WbEligibilityQuestionsPage { get; set; }
        internal virtual Pages.Wb.AddressDetailsPage WbAddressDetailsPage { get; set; }
        internal virtual Pages.Wb.BusinessAccountPage WbBusinessAccountDetailsPage { get; set; }
        internal virtual Pages.Wb.PersonalBankAccountDetailsPage WbPersonalBankAccountPage { get; set; }
        internal virtual Pages.Wb.PersonalDebitCardPage WbPersonalDebitCardDetailsPage { get; set; }
        internal virtual Pages.Wb.BusinessDetailsPage WbBusinessDetailsPage { get; set; }
        internal virtual Pages.Wb.AdditionalDirectorsPage WbAdditionalDirectorsPage { get; set; }
        internal virtual Pages.Wb.BusinessBankAccountPage WbBusinessBankAccountPage { get; set; }
        internal virtual Pages.Wb.BusinessDebitCardPage WbBusinessDebitCardPage { get; set; }
        internal virtual Pages.Wb.AcceptedPage WbAcceptedPage { get; set; }

        #endregion

        #region CommomPages

        internal virtual Pages.PersonalDetailsPage PersonalDetailsPage { get; set; }
        internal virtual Pages.ProcessingPage ProcessingPage { get; set; }
        internal virtual Pages.DealDonePage DealDonePage { get; set; }

        #endregion
    }
}
