using System;
using System.Collections.Generic;
using Gallio.Framework.Pattern;

namespace Wonga.QA.Tests.Core
{
    [AttributeUsage(PatternAttributeTargets.TestComponent, AllowMultiple = true)]
    public class JIRAAttribute : MetadataPatternAttribute
    {
        private String _issue;

        public JIRAAttribute(String issue)
        {
            _issue = issue;
        }

        protected override IEnumerable<KeyValuePair<String, String>> GetMetadata()
        {
            yield return new KeyValuePair<String, String>("JIRA", _issue);
        }
    }
}
