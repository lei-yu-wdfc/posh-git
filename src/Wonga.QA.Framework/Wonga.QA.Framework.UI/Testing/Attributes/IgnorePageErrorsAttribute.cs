using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Common.Reflection;
using Gallio.Framework.Pattern;


namespace Wonga.QA.Framework.UI.Testing.Attributes
{
    [AttributeUsage(PatternAttributeTargets.Test)]
    public class IgnorePageErrorsAttribute : TestDecoratorPatternAttribute
    {
        protected override void DecorateTest(IPatternScope scope, ICodeElementInfo codeElement)
        {
            scope.TestBuilder.AddMetadata("IgnorePageErrors", true.ToString());
        }
    }
}
