using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium.Remote;

namespace Wonga.QA.Framework.UI
{
    public class CustomRemoteWebDriver : RemoteWebDriver
    {
        public CustomRemoteWebDriver(Uri uri, DesiredCapabilities capabilities) : base(uri, capabilities)
        {
        }

        public SessionId GetSessionId()
        {
            return SessionId;
        }
    }
}
