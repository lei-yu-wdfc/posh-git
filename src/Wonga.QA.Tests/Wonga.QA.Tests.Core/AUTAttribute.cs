using System;
using System.Collections.Generic;
using Gallio.Common.Reflection;
using Gallio.Framework;
using Gallio.Framework.Pattern;
using Gallio.Model;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Tests.Core
{
    [AttributeUsage(PatternAttributeTargets.Test)]
    public class AUTAttribute : TestDecoratorPatternAttribute
    {
        public List<AUT> AUTs;

        public AUTAttribute(AUT aut, params AUT[] auts)
        {
            AUTs = new List<AUT>(auts) { aut };
        }

        protected override void DecorateTest(IPatternScope scope, ICodeElementInfo codeElement)
        {
            AUTs.ForEach(aut => scope.TestBuilder.AddMetadata(MetadataKeys.Category, aut.ToString()));
            scope.TestBuilder.TestActions.BeforeTestChain.Before(state =>
            {
                if (!AUTs.Contains(Config.AUT))
                    throw new SilentTestException(TestOutcome.Skipped);
            });
            
        }
    }
}
