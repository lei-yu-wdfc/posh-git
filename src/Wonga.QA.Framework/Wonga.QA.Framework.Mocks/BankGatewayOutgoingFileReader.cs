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

        public bool VerifyPinSentForBankAccountNumber(string bankAccountNumber, string pinNumber)
        {
            byte[] fileData = _dataDriver.QaData.Db.OutgoingBankGatewayFile.FindByBankAccountNumber(bankAccountNumber).FileData;

            var fileDataString = System.Text.Encoding.ASCII.GetString(fileData);

            return fileDataString.Contains(pinNumber);
        }
    }
}
