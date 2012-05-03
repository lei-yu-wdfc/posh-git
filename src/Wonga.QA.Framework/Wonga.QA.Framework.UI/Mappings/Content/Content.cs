using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.Mappings.Content.Agreements;

namespace Wonga.QA.Framework.UI
{
    public class Content
    {
        private static Dictionary<CultureInfo, Content> Contents = new Dictionary<CultureInfo, Content>();
        private static object _lock = new object();
        private static CultureInfo _cultureInfo;

        public static Content Get
        {
            get
            {
                lock (_lock)
                {
                    _cultureInfo = CultureInfo.GetCultureInfo("en-US");//Thread.CurrentThread.CurrentUICulture);
                    if (!Contents.ContainsKey(_cultureInfo))
                        Contents.Add(_cultureInfo, new Content(_cultureInfo));
                }

                return Contents[_cultureInfo];
            }
        }

        private string _xmlFileName;
        private XmlMapper _xmlMapper;

        public Content(CultureInfo cultureInfo)
        {
            _xmlFileName =
                string.Format(string.Format("Wonga.QA.Framework.UI.Mappings.Xml.Content.{0}.{1}.xml", Config.AUT,
                                            cultureInfo.TwoLetterISOLanguageName));
            _xmlMapper = new XmlMapper(_xmlFileName, _xmlFileName);
            _xmlMapper.GetValues(this, null);
        }

#region Content
        public string YourDetails { get; set; }
        public String ProblemProcessingDetailsMessage { get; set; }
        public String PasswordWarningMessage { get; set; }
        public String ApplicationErrorMessage { get; set; }
        public LoanAgreement LoanAgreement { get; set; }
#endregion
    }
}
