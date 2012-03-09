using System;
using System.Collections.Generic;
using System.Globalization;
using Wonga.QA.Tests.Payments.Helpers.Ca;

namespace Wonga.QA.Tests.Payments.Helpers.Ca
{
    public static class ScoitaFileHelpers
    {
        private const char _customerBatchHeaderRecordLine = 'B';

         public static void AddOnlineBillPaymentFileToMock(string fileName, IEnumerable<OnlineBillPaymentTransaction> transactionsToInclude)
         {
             return;
         }
    }

	public class OnlineBillPaymentTransaction
	{
        private const char CustomerDetailRecord = 'R';
        private const int ItemNumberLenght = 6;
        private const int BatchNumberLenght = 4;
        private const int AmountLenght = 12;
        private const int CustomerAccountNumberLenght = 30;
        private const int RemittanceTraceNumberLenght = 30;
        private const int CustomerFullNameLenght = 35;

	    private int _itemNumber;
        private int _batchNumber;
        private long _amount;
        private string _customerAccountNumber;
        private string _remittanceTraceNumber;
        private string _customerFullName;
	    
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

	    public long Amount
	    {
	        get { return _amount; }
	        set { _amount = value.AssertLenght(AmountLenght); }
	    }

	    public string CustomerAccountNumber
	    {
	        get { return _customerAccountNumber; }
	        set { _customerAccountNumber = value.AssertLenght(CustomerAccountNumberLenght); }
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
	    
	    private string ToFileFormat()
	    {
	        return
	            string.Format(
	                "{0}{1}{2}{3}{4}{5}{6}{7}",
	                CustomerDetailRecord, ItemNumber.ToStringWithPadLeft(ItemNumberLenght),
	                BatchNumber.ToStringWithPadLeft(BatchNumberLenght), Amount.ToStringWithPadLeft(AmountLenght),
                    CustomerAccountNumber.PadLeft(CustomerAccountNumberLenght),
	                RemittancePaymentDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture),
                    RemittanceTraceNumber.PadLeft(RemittanceTraceNumberLenght), CustomerFullName.PadLeft(CustomerFullNameLenght));
	    }

	    public override string ToString()
	    {
	        return
	            string.Format(
	                "ItemNumber: {0}, BatchNumber: {1}, Amount: {2}, CustomerAccountNumber: {3}, RemittancePaymentDate: {4}, RemittanceTraceNumber: {5}, CustomerFullName: {6}",
	                ItemNumber, BatchNumber, Amount, CustomerAccountNumber, RemittancePaymentDate, RemittanceTraceNumber,
	                CustomerFullName);
	    }

        

	}

    
}