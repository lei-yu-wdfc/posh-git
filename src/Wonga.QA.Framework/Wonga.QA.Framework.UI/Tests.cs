using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Resources;
using System.Reflection;
using MbUnit.Framework;
using System.Threading;

namespace Wonga.QA.Framework.UI
{
    class Tests
    {
        [Test]
        private void Initialize()
        {
            var x = Mappings.Elements.Get.YourDetailsSection;
        }
    }
}
