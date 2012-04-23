using System;
using System.Linq;
using Gallio.Common.Reflection;
using Gallio.Framework;
using Gallio.Framework.Pattern;
using Gallio.Model;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Db.Ops;

namespace Wonga.QA.Tests.Core
{
    [AttributeUsage(PatternAttributeTargets.Test | PatternAttributeTargets.TestComponent)]
    public class FeatureSwitchAttribute : TestDecoratorPatternAttribute
    {
        private string _featureSwitchKey;

        public FeatureSwitchAttribute(string featureSwitchKey)
        {
            _featureSwitchKey = featureSwitchKey;
        }

        protected override void DecorateTest(IPatternScope scope, ICodeElementInfo codeElement)
        {
            ServiceConfigurationEntity entity = Drive.Db.Ops.ServiceConfigurations.Single(sc => sc.Key == _featureSwitchKey);

            if (entity == null)
            {
                return;
            }
            
            if (bool.Parse(entity.Value))
            {
                // Enabled
                return;
            }
            
            scope.TestBuilder.TestActions.BeforeTestChain.Before(state =>
            {               
                throw new SilentTestException(TestOutcome.Skipped);
            });
        }
    }
}