using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using Gallio.Framework.Pattern;

namespace Wonga.QA.Framework.UI.Testing.Attributes
{
    [AttributeUsage(PatternAttributeTargets.Test)]
    public class IgnorePageErrorsAttribute : TestDecoratorPatternAttribute
    {
    }
}
