﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Mappings.Elements;
using Wonga.QA.Framework.Mobile.Mappings.Pages;
using Wonga.QA.Framework.Mobile.Mappings.Sections;
using Wonga.QA.Framework.Mobile.Mappings.Xml;

namespace Wonga.QA.Framework.Mobile.Mappings.Ui
{
    public class UiMapMobile
    {
        private static UiMapMobile MyElements;
        private static object _lock = new object();

        public static UiMapMobile Get
        {
            get
            {
                lock (_lock)
                {
                    if (MyElements == null)
                    {
                        MyElements = new UiMapMobile();
                    }
                }
                return MyElements;
            }
        }

        protected UiMapMobile()
        {
            XmlMapper = new XmlMapperMobile("Wonga.QA.Framework.Mobile.Mappings.Xml.Ui._base.xml", string.Format("Wonga.QA.Framework.Mobile.Mappings.Xml.Ui.{0}.xml", Config.AUT));
            XmlMapper.GetValues(this, null);
        }

        public XmlMapperMobile XmlMapper = null;

        #region Elements
        public virtual SlidersElement SlidersElement { get; set; }
        public virtual TabsElementMobile TabsElementMobile { get; set; }
        public virtual InternationalElement InternationalElement { get; set; }
        public virtual LoginElement LoginElement { get; set; }
        public virtual HelpElement HelpElement { get; set; }
        public virtual FAQElement FAQElement { get; set; }
        public virtual TabsElement TabsElement { get; set; }
        public virtual TopupSlidersElement TopupSlidersElement { get; set; }
        public virtual MobiScrollElement MobiScrollElement { get; set; }
        public virtual DayMonthYearMobiScrollElement DayMonthYearMobiScrollElement { get; set; }
        public virtual MonthYearMobiScrollElement MonthYearMobiScrollElement { get; set; }
        #endregion

        #region Pages

        public virtual AddressDetailsPageMobile AddressDetailsPageMobile { get; set; }
        public virtual PersonalDetailsPageMobile PersonalDetailsPageMobile { get; set; }
        public virtual AccountDetailsPageMobile AccountDetailsPageMobile { get; set; }
        public virtual ExtensionAgreementPageMobile ExtensionAgreementPageMobile { get; set; }
        public virtual PersonalBankAccountPageMobile PersonalBankAccountPageMobile { get; set; }
        public virtual AcceptedPageMobile AcceptedPage { get; set; }
        public virtual ApplyTermsPageMobile ApplyTermsPage { get; set; }
        public virtual DealDonePage DealDonePage { get; set; }
        public virtual MySummaryPageMobile MySummaryPage { get; set; }
        public virtual DeclinedPageMobile DeclinedPage { get; set; }
        public virtual PersonalDebitCardPageMobile PersonalDebitCardPageMobile { get; set; }
        public virtual LoginPageMobile LoginPageMobile { get; set; }
        public virtual ApplyPageMobile ApplyPageMobile { get; set; }
        public virtual MyPersonalDetailsPageMobile MyPersonalDetailsPageMobile { get; set; }
        public virtual TopupRequestPage TopupRequestPage { get; set; }
        public virtual TopUpDealDonePage TopUpDealDonePage { get; set; }
        public virtual TopupAcceptPageMobile TopupAcceptPageMobile { get; set; }
        #endregion

        #region Sections

        public virtual ContactingYouSection ContactingYouSection { get; set; }
        public virtual EmploymentDetailsSection EmploymentDetailsSection { get; set; }
        public virtual YourDetailsSection YourDetailsSection { get; set; }
        public virtual YourNameSection YourNameSection { get; set; }
        public virtual BankAccountSection BankAccountSection { get; set; }
        public virtual MobilePinVerificationSection MobilePinVerificationSection { get; set; }
        public virtual AccountDetailsSection AccountDetailsSection { get; set; }
        public virtual DebitCardSection DebitCardSection { get; set;}
        public virtual ApplicationSection ApplicationSection { get; set; }
        #endregion
    }
}
