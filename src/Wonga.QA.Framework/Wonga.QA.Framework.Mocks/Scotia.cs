using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Wonga.QA.Framework.Db;
using Wonga.QA.Framework.Db.QaData;
using Wonga.QA.Framework.Mocks.Entities;

namespace Wonga.QA.Framework.Mocks
{
    public class Scotia
    {
        private readonly DbDriver _dbDriver;
        private const string OnlineBillPaymentRemittanceReport = "EBBADATA";
    	private const string GatewayName = "Scotia";

    	public Scotia()
        {
			_dbDriver = new DbDriver();
        }

		public ScotiaSetupBuilder Setup()
		{
			return new ScotiaSetupBuilder();
		}

    	public void AddOnlineBillPaymentFile(string fileId,
                                                          IEnumerable<OnlineBillPaymentTransaction>
                                                              transactionsToInclude)
        {
            OnlineBillPaymentRemittanceReportCustomerBatch batch =
                OnlineBillPaymentRemittanceReportCustomerBatch.CreateWith(transactionsToInclude);

            IncomingBankGatewayFile file = CreateFrom(fileId, batch);

            _dbDriver.QaData.IncomingBankGatewayFiles.InsertOnSubmit(file);
            file.Submit();
        }

        private IncomingBankGatewayFile CreateFrom(string fileId, OnlineBillPaymentRemittanceReportCustomerBatch batch)
        {
            byte[] file = batch.ToFileFormat();

            return new IncomingBankGatewayFile { FileData = file, FileName = string.Format("{0}.{1}", OnlineBillPaymentRemittanceReport, fileId), Gateway = GatewayName };
        }

        #region Nested type: OnlineBillPaymentRemittanceReportCustomerBatch

        private class OnlineBillPaymentRemittanceReportCustomerBatch
        {
            private OnlineBillPaymentRemittanceReportCustomerBatchHeaderRecord _header;

            private List<OnlineBillPaymentTransaction> _transactions;

            public static OnlineBillPaymentRemittanceReportCustomerBatch CreateWith(
                IEnumerable<OnlineBillPaymentTransaction> transactions)
            {
                var header = OnlineBillPaymentRemittanceReportCustomerBatchHeaderRecord.Create();

                return new OnlineBillPaymentRemittanceReportCustomerBatch
                           {_header = header, _transactions = new List<OnlineBillPaymentTransaction>(transactions)};
            }

            public byte[] ToFileFormat()
            {
                var stringBuilder = new StringBuilder(_header.ToFileFormat());

                foreach (var transaction in _transactions)
                {
                    transaction.BatchNumber = _header.BatchNumber;
                    stringBuilder.AppendLine(transaction.ToFileFormat());
                }

                return Encoding.ASCII.GetBytes(stringBuilder.ToString());
            }
        }

        #endregion

        #region Nested type: OnlineBillPaymentRemittanceReportCustomerBatchHeaderRecord

        private class OnlineBillPaymentRemittanceReportCustomerBatchHeaderRecord
        {
            private const int BatchNumberLenght = 10;
            private const int ProcessingModeLenght = 1;
            private const int SebpLenght = 4;
            private const int CorporateCreditorIdentificationNumberLenght = 8;
            private const int InstitutionCodeLenght = 3;
            private const int ReceivingInstitutionNameLenght = 20;
            private const int BankAccountBeingCreditedLenght = 12;

            private const char RecordType = 'B';
            public int BatchNumber { get; private set; }
            public int ProcessingMode { get; private set; }
            public DateTime ProcessingDate { get; private set; }
            public DateTime PaymentDate { get; private set; }
            public string Sebp { get; private set; }
            public int CorporateCreditorIdentificationNumber { get; private set; }
            public int InstitutionCode { get; private set; }
            public string ReceivingInstitutionName { get; private set; }
            public string BankAccountBeingCredited { get; private set; }

            public static OnlineBillPaymentRemittanceReportCustomerBatchHeaderRecord Create()
            {
                var random = new Random();

                return new OnlineBillPaymentRemittanceReportCustomerBatchHeaderRecord
                {
                    BankAccountBeingCredited = "SomeWongaAccount",
                    BatchNumber = random.Next(9999),
                    CorporateCreditorIdentificationNumber = 999,
                    InstitutionCode = 001,
                    PaymentDate = DateTime.UtcNow,
                    ProcessingDate = DateTime.UtcNow,
                    ProcessingMode = 1,
                    ReceivingInstitutionName = "Wonga's Bank",
                    Sebp = "Sebp"
                };
            }

            public string ToFileFormat()
            {
                return
                    string.Format(
                        "{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}\n",
                        RecordType, BatchNumber.ToStringWithPadLeft(BatchNumberLenght),
                        ProcessingMode.ToStringWithPadLeft(ProcessingModeLenght),
                        ProcessingDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture),
                        PaymentDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture), Sebp.PadLeft(SebpLenght),
                        CorporateCreditorIdentificationNumber.ToStringWithPadLeft(
                            CorporateCreditorIdentificationNumberLenght),
                        InstitutionCode.ToStringWithPadLeft(InstitutionCodeLenght),
                        ReceivingInstitutionName.PadLeft(ReceivingInstitutionNameLenght),
                        BankAccountBeingCredited.PadLeft(BankAccountBeingCreditedLenght));
            }

        }

        #endregion
    }
}
