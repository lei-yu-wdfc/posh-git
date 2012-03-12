using System;
using System.Globalization;

namespace Wonga.QA.Framework.Mocks.Entities
{
    public class OnlineBillPaymentTransaction
    {
        private const char RecordType = 'R';
        private const int ItemNumberLenght = 6;
        private const int BatchNumberLenght = 4;
        private const int AmountLenght = 12;
        private const int CcinLenght = 30;
        private const int RemittanceTraceNumberLenght = 30;
        private const int CustomerFullNameLenght = 35;

        private long _amountInCent;
        private int _batchNumber;
        private string _ccin;
        private string _customerFullName;
        private int _itemNumber;
        private string _remittanceTraceNumber;

        public int ItemNumber
        {
            get { return _itemNumber; }
            set { _itemNumber = value.AssertLenght(ItemNumberLenght); }
        }

        public int BatchNumber
        {
            get { return _batchNumber; }
            set { _batchNumber = value.AssertLenght(BatchNumberLenght); }
        }

        public long AmountInCent
        {
            get { return _amountInCent; }
            set { _amountInCent = value.AssertLenght(AmountLenght); }
        }

        public string Ccin
        {
            get { return _ccin; }
            set { _ccin = value.AssertLenght(CcinLenght); }
        }

        public string RemittanceTraceNumber
        {
            get { return _remittanceTraceNumber; }
            set { _remittanceTraceNumber = value.AssertLenght(RemittanceTraceNumberLenght); }
        }

        public string CustomerFullName
        {
            get { return _customerFullName; }
            set { _customerFullName = value.AssertLenght(CustomerFullNameLenght); }
        }

        public DateTime RemittancePaymentDate { get; set; }

        public string ToFileFormat()
        {
            return
                string.Format(
                    "{0}{1}{2}{3}{4}{5}{6}{7}\n",
                    RecordType, ItemNumber.ToStringWithPadLeft(ItemNumberLenght),
                    BatchNumber.ToStringWithPadLeft(BatchNumberLenght), AmountInCent.ToStringWithPadLeft(AmountLenght),
                    Ccin.PadLeft(CcinLenght),
                    RemittancePaymentDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture),
                    RemittanceTraceNumber.PadLeft(RemittanceTraceNumberLenght),
                    CustomerFullName.PadLeft(CustomerFullNameLenght));
        }

        public override string ToString()
        {
            return
                string.Format(
                    "ItemNumber: {0}, BatchNumber: {1}, AmountInCent: {2}, Ccin: {3}, RemittancePaymentDate: {4}, RemittanceTraceNumber: {5}, CustomerFullName: {6}",
                    ItemNumber, BatchNumber, AmountInCent, Ccin, RemittancePaymentDate, RemittanceTraceNumber,
                    CustomerFullName);
        }
    }
}