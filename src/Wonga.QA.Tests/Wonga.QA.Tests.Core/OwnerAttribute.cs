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
    public class OwnerAttribute : TestDecoratorPatternAttribute
    {
        public List<Owner> Owners;

        public OwnerAttribute(Owner owner, params Owner[] owners)
        {
            Owners = new List<Owner>(owners) { owner };
        }

        protected override void DecorateTest(IPatternScope scope, ICodeElementInfo codeElement)
        {
            Owners.ForEach(Owner => scope.TestBuilder.AddMetadata(MetadataKeys.Category, Owner.ToString()));
        }
    }
}
