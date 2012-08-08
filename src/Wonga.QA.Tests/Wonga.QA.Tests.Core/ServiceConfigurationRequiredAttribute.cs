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

            scope.TestBuilder.TestActions.BeforeTestChain.Before(state =>
            {
                var setting = Drive.Data.Ops.GetServiceConfiguration<string>(serviceConfigurationKey);

                if (setting == null)
                {
                    if (_expectedValue != null)
                    
                        throw new SilentTestException(TestOutcome.Pending,
                            string.Format("The service configuration required for key {0} is not found",
                                                      serviceConfigurationKey));
                    else                    
                        return;

                }
                else
                {
                    if (setting != _expectedValue)
                        throw new SilentTestException(TestOutcome.Pending,
                             string.Format("The service configuration found for key {0} is not equal with the expected value. " +
                                                          "The value found is {1}.The expected value is {2}.",
                                                          serviceConfigurationKey,setting, _expectedValue));
                    else
                        return;
                }
            });
        }
    }
}
