using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.UI.Mappings;
using Wonga.QA.Framework.UI.Mappings.ContentItems;

namespace Wonga.QA.Framework.UI
{
    public class Content
    {
        private static Dictionary<CultureInfo, Content> Contents = new Dictionary<CultureInfo, Content>();
        private static object _lock = new object();

        public static Content Get(CultureInfo cultureInfo)
        {
            lock(_lock)
            {
                if (!Contents.ContainsKey(cultureInfo))
                    Contents.Add(cultureInfo, new Content(cultureInfo));
            }

            return Contents[cultureInfo];
        }

        private string _xmlFileName;
        private XmlMapper _xmlMapper;

        public Content(CultureInfo cultureInfo)
        {
            _xmlFileName =
                string.Format(string.Format("Wonga.QA.Framework.UI.Mappings.Xml.Content.{0}.{1}.xml", Config.AUT,
                                            cultureInfo.TwoLetterISOLanguageName));
            _xmlMapper = new XmlMapper(_xmlFileName);
            _xmlMapper.GetValues(this, null);
        }

#region Content
        public string YourDetails { get; set; }
        public LoanAgreement LoanAgreement { get; set; }
#endregion
    }
}
