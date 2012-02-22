using System;
using System.Collections.Generic;
using Gallio.Framework.Pattern;

namespace Wonga.QA.Tests.Core
{
    [AttributeUsage(PatternAttributeTargets.TestComponent, AllowMultiple = true)]
    public class JIRAAttribute : MetadataPatternAttribute
    {
        private String _key;

        public JIRAAttribute(String key)
        {
            _key = key;
        }

        protected override IEnumerable<KeyValuePair<String, String>> GetMetadata()
        {
            yield return new KeyValuePair<String, String>("JIRA", String.Format("https://jira.wonga.com/browse/{0}", _key));
        }
    }
}
