using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Mobile.Mappings.Content.Elements;
using Wonga.QA.Framework.Mobile.Mappings.Content.Sections;
using Wonga.QA.Framework.Mobile.Mappings.Xml;

namespace Wonga.QA.Framework.Mobile.Mappings.Content
{
    class ContentMapMobile
    {
        private static Dictionary<CultureInfo, ContentMapMobile> Contents = new Dictionary<CultureInfo, ContentMapMobile>();
        private static object _lock = new object();
        private static CultureInfo _cultureInfo;

        public static ContentMapMobile Get
        {
            get
            {
                lock (_lock)
                {
                    _cultureInfo = CultureInfo.GetCultureInfo("en-US");//Thread.CurrentThread.CurrentUICulture);
                    if (!Contents.ContainsKey(_cultureInfo))
                        Contents.Add(_cultureInfo, new ContentMapMobile(_cultureInfo));
                }

                return Contents[_cultureInfo];
            }
        }

        private string _xmlFileName;
        private XmlMapperMobile _xmlMapper;

        public ContentMapMobile(CultureInfo cultureInfo)
        {
            _xmlFileName =
                string.Format(string.Format("Wonga.QA.Framework.Mobile.Mappings.Xml.Content.{0}.{1}.xml", Config.AUT,
                                            cultureInfo.TwoLetterISOLanguageName));
            _xmlMapper = new XmlMapperMobile(_xmlFileName, _xmlFileName);
            _xmlMapper.GetValues(this, null);
        }

        public virtual TabsElementMobile TabsElementMobile { get; set; }
        public virtual YourDetailsSection YourDetailsSection { get; set; }
        public virtual MobilePinVerificationSection MobilePinVerificationSection { get; set; }
        public virtual HelpElement HelpElement { get; set; }
        public virtual AccountDetailsSection AccountDetailsSection { get; set; }
    }
}
