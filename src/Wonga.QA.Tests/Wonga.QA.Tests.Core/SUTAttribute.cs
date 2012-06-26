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
    public class SUTAttribute : TestDecoratorPatternAttribute
    {
        public List<SUT> SUTs;

        public SUTAttribute(SUT sut, params SUT[] suts)
        {
            SUTs = new List<SUT>(suts) { sut };
        }

        protected override void DecorateTest(IPatternScope scope, ICodeElementInfo codeElement)
        {
            SUTs.ForEach(SUT => scope.TestBuilder.AddMetadata(MetadataKeys.SUT, SUT.ToString()));
            scope.TestBuilder.TestActions.BeforeTestChain.Before(state =>
            {
                if (!SUTs.Contains(Config.SUT))
                    throw new SilentTestException(TestOutcome.Skipped);
            });
            
        }
    }
}
