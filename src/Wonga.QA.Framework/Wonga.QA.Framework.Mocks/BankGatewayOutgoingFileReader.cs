using System;
using Wonga.QA.Framework.Data;

namespace Wonga.QA.Framework.Mocks
{
    public class BankGatewayOutgoingFileReader
    {
        private readonly DataDriver _dataDriver;

        public BankGatewayOutgoingFileReader()
        {
            _dataDriver = new DataDriver();
        }

        public bool VerifyPinContainedInBankGatewayFileSent(string bankAccountNumber, string pinNumber)
        {
            // There will be two files, one for cash-out and one for cash-in. Find the cash-in file, this will, be the one with the highest id
            byte[] fileData = _dataDriver.QaData.Db.OutgoingBankGatewayFile.FindByBankAccountNumber(bankAccountNumber).FileData;

            var fileDataString = System.Text.Encoding.ASCII.GetString(fileData);

            // look for Wonga.ca (%pin%)
            var startIndex = fileDataString.IndexOf("Wonga.ca (", StringComparison.InvariantCultureIgnoreCase);
            var endIndex = fileDataString.IndexOf(")", StringComparison.InvariantCultureIgnoreCase);
            var filePin = fileDataString.Substring(startIndex, endIndex - startIndex);

            return pinNumber.Equals(filePin);
        }
    }
}
