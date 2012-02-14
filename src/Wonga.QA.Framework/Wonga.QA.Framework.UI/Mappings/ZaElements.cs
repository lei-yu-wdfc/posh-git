using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wonga.QA.Framework.UI.Mappings.Sections;

namespace Wonga.QA.Framework.UI.Mappings
{
    internal sealed class ZaElements : BaseElements
    {
        internal ZaElements()
        {
            YourDetailsElement = new YourDetailsElement {Dependants = "xx"};
            YourNameElement = new YourNameElement{FirstName = "xcx"};
        }
    }
}
