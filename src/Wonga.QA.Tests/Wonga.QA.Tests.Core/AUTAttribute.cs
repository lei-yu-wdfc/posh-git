﻿using System;
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
        private List<AUT> _auts;

        public AUTAttribute(AUT aut, params AUT[] auts)
        {
            _auts = new List<AUT>(auts) { aut };
        }

        protected override void DecorateTest(IPatternScope scope, ICodeElementInfo codeElement)
        {
            _auts.ForEach(aut => scope.TestBuilder.AddMetadata(MetadataKeys.Category, aut.ToString()));
            scope.TestBuilder.TestActions.BeforeTestChain.Before(state =>
            {
                if (!_auts.Contains(Config.AUT))
                    throw new SilentTestException(TestOutcome.Skipped);
            });
            
        }
    }
}
