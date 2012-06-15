using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Mappings.Pages;
using Wonga.QA.Framework.Mobile.Mappings.Sections;
using Wonga.QA.Framework.Mobile.Mappings.Ui.Elements;
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
        #endregion

        #region Pages

        public virtual AddressDetailsPage AddressDetailsPage { get; set; }
        public virtual PersonalDetailsPage PersonalDetailsPage { get; set; }
        #endregion

        #region Sections

        public virtual ContactingYouSection ContactingYouSection { get; set; }
        public virtual EmploymentDetailsSection EmploymentDetailsSection { get; set; }
        public virtual YourDetailsSection YourDetailsSection { get; set; }
        public virtual YourNameSection YourNameSection { get; set; }
        #endregion
    }
}
