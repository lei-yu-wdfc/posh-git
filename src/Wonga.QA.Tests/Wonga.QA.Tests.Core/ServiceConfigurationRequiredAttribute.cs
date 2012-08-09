using System;
using System.Collections.Generic;
using Gallio.Common.Reflection;
using Gallio.Framework;
using Gallio.Framework.Pattern;
using Gallio.Model;
using Wonga.QA.Framework;
using Wonga.QA.Framework.Core;
using Wonga.QA.Framework.Data;

namespace Wonga.QA.Tests.Core
{
    [AttributeUsage(PatternAttributeTargets.Test)]
    public class ServiceConfigurationRequiredAttribute : TestDecoratorPatternAttribute
    {
        private ServiceConfigurationKeys _serviceConfigurationKey;
        private String _expectedValue;



        public ServiceConfigurationRequiredAttribute(ServiceConfigurationKeys serviceConfigurationKey, string expectedValue)
        {
            _serviceConfigurationKey = serviceConfigurationKey;
            _expectedValue = expectedValue;
        }

        protected override void DecorateTest(IPatternScope scope, ICodeElementInfo codeElement)
        {
            string serviceConfigurationKey = Get.EnumToString(_serviceConfigurationKey);
            scope.TestBuilder.AddMetadata(serviceConfigurationKey, _expectedValue);

            scope.TestBuilder.TestActions.InitializeTestChain.After(state =>
            {
                var setting = Drive.Data.Ops.GetServiceConfiguration<string>(serviceConfigurationKey);
                if (setting == null)
                {
                    if (_expectedValue != null)
                        throw new SilentTestException(TestOutcome.Canceled,
                                                      string.Format(
                                                          "Required service configuration of '{0}' was not found",
                                                          serviceConfigurationKey));
                }
                else
                {
                    if (setting != _expectedValue)
                        throw new SilentTestException(TestOutcome.Canceled,
                             string.Format("Required service configuration of '{0}' was found with value '{1}' and did not match the required '{2}'",
                                                          serviceConfigurationKey,setting, _expectedValue));
                }
            });
        }
    }
}
