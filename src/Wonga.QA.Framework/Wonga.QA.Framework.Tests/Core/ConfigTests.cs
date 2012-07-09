using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Framework.Tests.Core
{
    [Explicit]
    [TestFixture]
    public class ConfigTests
    {
        [Test]
        public void ItCanReadUiHomePage()
        {
            Assert.IsNotNull(ConfigFromConfig.Ui.Home);
        }
    }
}
