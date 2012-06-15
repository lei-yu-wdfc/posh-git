using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.Core;
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
    }
}
