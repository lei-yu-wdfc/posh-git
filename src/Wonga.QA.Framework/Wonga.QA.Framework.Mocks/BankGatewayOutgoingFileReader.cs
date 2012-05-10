using System;
using Simple.Data;
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
            // There will be two files, one for cash-out and one for cash-in. The cash-in file contains the pin. It is one one with the highest id.
            var files = _dataDriver.QaData.Db.OutgoingBankGatewayFile.FindAllByBankAccountNumber(bankAccountNumber);

            int maxId = 0;
            dynamic maxFile = null;

            foreach (var file in files)
            {
                if (file.Id > maxId)
                {
                    maxId = file.Id;
                    maxFile = file;
                }
            }

            if (maxFile == null)
            {
                return false;
            }

            string fileDataString = System.Text.Encoding.ASCII.GetString(maxFile.FileData);

            // look for Wonga.ca (%pin%)
            const int originatorShortNameStartIndex = 89;
            const int originatorShortNameLenght = 15;
            const string originatorShortNameFormat = "Wonga.ca ({0})";

            const int originatorLongNameStartIndex = 134;
            const int originatorLongNameLenght = 19;
            const string originatorLongNameFormat = "Wonga Canada ({0})";

            var detail = fileDataString.Split(new [] { Environment.NewLine }, StringSplitOptions.None)[1];

            string originatorShortName = detail.Substring(originatorShortNameStartIndex, originatorShortNameLenght);
            string originatorLongName = detail.Substring(originatorLongNameStartIndex, originatorLongNameLenght);

            if (originatorShortName != string.Format(originatorShortNameFormat, pinNumber))
            {
                return false;
            }

            if (originatorLongName != string.Format(originatorLongNameFormat, pinNumber))
            {
                return false;
            }

            return true;
        }
    }
}
