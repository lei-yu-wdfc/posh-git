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
        private readonly string _featureSwitchKey;
        private readonly bool _runIfDisabled = false;
        private readonly dynamic _opsServiceConfig = Drive.Data.Ops.Db.ServiceConfigurations;

        public FeatureSwitchAttribute(string featureSwitchKey)
        {
            _featureSwitchKey = featureSwitchKey;
        }

        public FeatureSwitchAttribute(string featureSwitchKey, bool runIfDisabled)
        {
            _featureSwitchKey = featureSwitchKey;
            _runIfDisabled = runIfDisabled;
        }

        protected override void DecorateTest(IPatternScope scope, ICodeElementInfo codeElement)
        {            
            scope.TestBuilder.TestActions.BeforeTestChain.Before(state =>
            {
                ServiceConfigurationEntity entity = _opsServiceConfig.FindByKey(_featureSwitchKey);

                if (entity == null && !_runIfDisabled)
                {
                    throw new SilentTestException(TestOutcome.Skipped,
                                                  string.Format("Could not find a feature switch value for key {0}",
                                                                _featureSwitchKey));
                }
                if (entity == null && _runIfDisabled)
                {
                    return;
                }
                if (!bool.Parse(entity.Value) && !_runIfDisabled)
                {
                    throw new SilentTestException(TestOutcome.Skipped,
                                                  string.Format("Feature switch value for key {0} is false",
                                                                _featureSwitchKey));
                }
                if (!bool.Parse(entity.Value) && _runIfDisabled)
                {
                    return;
                }
                if (bool.Parse(entity.Value) && _runIfDisabled)
                {
                    throw new SilentTestException(TestOutcome.Skipped,
                                                  string.Format("Feature switch value for key {0} is false",
                                                                _featureSwitchKey));
                }
            });
        }
    }
}