using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wonga.QA.Framework.ThirdParties
{
    /// <summary>
    /// Third party driver
    /// </summary>
    public class ThirdPartyDriver
    {
        /// <summary>
        /// Lazy ExactTarget object
        /// </summary>
        private readonly Lazy<ExactTarget> _exactTarget = new Lazy<ExactTarget>();

        /// <summary>
        /// Lazy Salesforce object
        /// </summary>
        private readonly Lazy<Salesforce> _salesforce = new Lazy<Salesforce>();

        /// <summary>
        /// ExactTarget. Instantiated lazily.
        /// </summary>
        public ExactTarget ExactTarget
        {
            get { return _exactTarget.Value; }
        }

        /// <summary>
        /// Salesforce. Instantiated lazily.
        /// </summary>
        public Salesforce Salesforce
        {
            get { return _salesforce.Value; }
        }
    }
}
