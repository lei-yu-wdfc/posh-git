
using System;
using Wonga.QA.Framework.Core;
namespace Wonga.QA.Framework.UI.Mappings
{
    public class Elements
    {
        private static Elements MyElements;
        private static object _lock = new object();

        public static Elements Get
        {
            get
            {
                lock (_lock)
                {
                    if (MyElements == null)
                    {
                        MyElements = new Elements();
                    }
                }
                return MyElements;
            }
        }

        protected Elements()
        {
           XmlMapper = new XmlMapper(string.Format("Wonga.QA.Framework.UI.Mappings.Xml.Elements.{0}.xml", Config.AUT));
           XmlMapper.GetValues(this, null);
        }

        public XmlMapper XmlMapper = null;

        public virtual Sections.YourNameSection YourNameSection { get; set; }
        public virtual Sections.YourDetailsSection YourDetailsSection { get; set; }
        public virtual Sections.MobilePinVerificationSection MobilePinVerificationSection { get; set; }
        public virtual Sections.ContactingYouSection ContactingYouSection { get; set; }
        public virtual Sections.SliderSection SliderSection { get; set; }
        public virtual Sections.AccountDetailsSection AccountDetailsSection { get; set; }
        public virtual Sections.DebitCardSection DebitCardSection { get; set; }
        public virtual Sections.BankAccountSection BankAccountSection { get; set; }
        public virtual Sections.EmploymentDetailsSection EmploymentDetailsSection { get; set; }

        #region WbPages

        public virtual Pages.Wb.EligibilityQuestionsPage EligibilityQuestionsPage { get; set; }
        public virtual Pages.Wb.PersonalBankAccountDetailsPage PersonalBankAccountPage { get; set; }
        public virtual Pages.Wb.PersonalDebitCardPage PersonalDebitCardDetailsPage { get; set; }
        public virtual Pages.Wb.BusinessDetailsPage BusinessDetailsPage { get; set; }
        public virtual Pages.Wb.AdditionalDirectorsPage AdditionalDirectorsPage { get; set; }
        public virtual Pages.Wb.AddAditionalDirectorsPage AddAditionalDirectorsPage { get; set; }
        public virtual Pages.Wb.BusinessDebitCardPage BusinessDebitCardPage { get; set; }
        public virtual Pages.Wb.AcceptedPage AcceptedPage { get; set; }

        #endregion

        #region CommomPages

        public virtual Pages.PersonalDetailsPage PersonalDetailsPage { get; set; }
        public virtual Pages.ProcessingPage ProcessingPage { get; set; }
        public virtual Pages.DealDonePage DealDonePage { get; set; }
        public virtual Pages.DeclinedPage DeclinedPage { get; set; }
        public virtual Pages.AddressDetailsPage AddressDetailsPage { get; set; }
        public virtual Pages.AccountDetailsPage AccountDetailsPage { get; set; }
        public virtual Pages.BankAccountPage BankAccountPage { get; set; }
        public virtual Pages.DebitCardPage DebitCardPage { get; set; }

        #endregion
    }
}
