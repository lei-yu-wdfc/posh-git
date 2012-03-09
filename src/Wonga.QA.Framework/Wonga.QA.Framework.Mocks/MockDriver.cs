using System;

namespace Wonga.QA.Framework.Mocks
{
    public class MockDriver
    {
        /// <summary>
        /// Lazy BankGatewayScotia object
        /// </summary>
        private readonly Lazy<BankGatewayScotia> _bankGatewayScotia = new Lazy<BankGatewayScotia>();

        /// <summary>
        /// BankGatewayScotia. Instantiated lazily.
        /// </summary>
        public BankGatewayScotia BankGatewayScotia
        {
            get { return _bankGatewayScotia.Value; }
        }

    }
}