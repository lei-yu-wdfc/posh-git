using System;

namespace Wonga.QA.Framework.Mocks.Entities
{
	public class ReturnItemNotificationChargebackDetailsRecord
	{
		public string RecordType { get; set; }
		public int RecordCount { get; set; }
		public long CustomerNumber { get; set; }
		public string ChargebackType { get; set; }
		public long Amount { get; set; }
		public DateTime DueDate { get; set; }
		public string OriginatorsCrossReferenceNumber { get; set; }
		public int ReturnBranchTransitNumber { get; set; }
		public string ReturnAccountNumber { get; set; }
		public ReturnItemNotificationChargebackDetailsContinuationRecord ContinuationRecord { get; set; }

		public override string ToString()
		{
			return string.Format(
				"RecordType={0}, RecordCount={1}, CustomerNumber={2}, ChargebackType={3}" +
				"Amount={4}, DueDate={5}, OriginatorsCrossReferenceNumber={6}, ReturnBranchTransitNumber={7}" +
				"ReturnAccountNumber={8}, ContinuationRecord=[{9}]",
				RecordType,
				RecordCount,
				CustomerNumber,
				ChargebackType,
				Amount,
				DueDate,
				OriginatorsCrossReferenceNumber,
				ReturnBranchTransitNumber,
				ReturnAccountNumber,
				ContinuationRecord);
		}
	}
}
