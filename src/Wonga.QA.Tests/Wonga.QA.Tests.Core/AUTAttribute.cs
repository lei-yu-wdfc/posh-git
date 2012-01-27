using System;
using System.Collections.Generic;
using System.Linq;
using Gallio.Framework.Pattern;
using Gallio.Model;
using Wonga.QA.Framework.Core;

namespace Wonga.QA.Tests.Core
{
    [AttributeUsage(PatternAttributeTargets.TestComponent)]
    public class AUTAttribute : MetadataPatternAttribute
    {
        private IEnumerable<AUT> _aut;

        public AUTAttribute(params AUT[] aut)
        {
            _aut = aut.Any() ? aut : Enum.GetValues(typeof(AUT)).Cast<AUT>();
        }

        protected override IEnumerable<KeyValuePair<String, String>> GetMetadata()
        {
            return _aut.Select(aut => new KeyValuePair<String, String>(MetadataKeys.Category, aut.ToString()));
        }
    }
}
