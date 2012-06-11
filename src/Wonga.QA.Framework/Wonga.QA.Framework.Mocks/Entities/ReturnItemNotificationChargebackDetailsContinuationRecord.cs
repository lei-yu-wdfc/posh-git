namespace Wonga.QA.Framework.Mocks.Entities
{
	public class ReturnItemNotificationChargebackDetailsContinuationRecord
	{
		public string RecordType { get; set; }
		public int RecordCount { get; set; }
		public long CustomerNumber { get; set; }
		public string RecipientsName { get; set; }
		public int RecipientsInstitutionCode { get; set; }
		public int RecipientsTransitNumber { get; set; }
		public string RecipientsAccountNumber { get; set; }
		public int ChargebackCodeNumber { get; set; }
		public string InvalidFields { get; set; }

		public override string ToString()
		{
			return string.Format(
				"RecordType={0}, RecordCount={1}, CustomerNumber={2}, RecipientsName={3}" +
				"RecipientsInstitutionCode={4}, RecipientsTransitNumber={5}, RecipientsAccountNumber={6}" +
				"ChargebackCodeNumber={7}, InvalidFields={8}",
				RecordType,
				RecordCount,
				CustomerNumber,
				RecipientsName,
				RecipientsInstitutionCode,
				RecipientsTransitNumber,
				RecipientsAccountNumber,
				ChargebackCodeNumber,
				InvalidFields);
		}
	}
}
