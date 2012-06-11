using System;

namespace Wonga.QA.Framework.Mocks.Entities
{
	public class ReturnItemNotificationHeader
	{
	    private const char RecordType = 'A';
	    public int RecordCount { get; private set; }
        public long CustomerNumber { get; private set; }
        public string OriginatorLongName { get; private set; }
        public DateTime ReportDate { get; private set; }
        public string ReportType { get; private set; }

        public static ReturnItemNotificationHeader Create()
        {
            var random = new Random();

            return new ReturnItemNotificationHeader
            {
                RecordCount = 1,
                CustomerNumber = 2
            };
        }

		public override string ToString()
		{
			return string.Format(
				"RecordType={0}, RecordCount={1}, CustomerNumber={2}, OriginatorLongName={3}," +
				"ReportDate={4}, ReportType={5}",
				RecordType,
				RecordCount,
				CustomerNumber,
				OriginatorLongName,
				ReportDate,
				ReportType);
		}
	}
}
