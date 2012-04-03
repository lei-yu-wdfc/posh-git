using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework.Assertions;
using MbUnit.Framework;

namespace Wonga.QA.Framework.UI
{
    public class SourceTestHelper
    {
        public static void SourceContainsTokens(UiClient client, List<KeyValuePair<string, string>> list)
        {
            foreach(var pair in list)
            {
                try
                {
                    Assert.IsTrue(client.Source().Contains(String.Format("{0}: {1}", pair.Key, pair.Value)));
                }
                catch(AssertionException exception)
                {
                    throw new AssertionException(String.Format("{0} - failed on {1}: {2}",exception.Message,pair.Key,pair.Value));
                }
            }
        }
    }
}
