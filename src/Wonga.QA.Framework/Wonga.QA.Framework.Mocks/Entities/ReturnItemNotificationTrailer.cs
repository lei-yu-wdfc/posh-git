namespace Wonga.QA.Framework.Mocks.Entities
{
	public class ReturnItemNotificationTrailer
	{
		public string RecordType { get; set; }
		public int RecordCount { get; set; }
		public long CustomerNumber { get; set; }
		public int TotalDebitItems { get; set; }
		public long TotalDebitAmount { get; set; }
		public int TotalCreditItems { get; set; }
		public long TotalCreditAmount { get; set; }
		public string ReportType { get; set; }

		public override string ToString()
		{
			return string.Format(
				"RecordType={0}, RecordCount={1}, CustomerNumber={2}," +
				"TotalDebitItems={3}, TotalDebitAmount={4}, TotalCreditItems={5}," +
				"TotalCreditAmount={6}, ReportType={7}",
				RecordType,
				RecordCount,
				CustomerNumber,
				TotalDebitItems,
				TotalDebitAmount,
				TotalCreditItems,
				TotalCreditAmount,
				ReportType);
		}
	}
}
