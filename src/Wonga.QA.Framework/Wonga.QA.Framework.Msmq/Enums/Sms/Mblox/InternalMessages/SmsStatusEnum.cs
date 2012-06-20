namespace Wonga.QA.Framework.Msmq.Enums.Sms.Mblox.InternalMessages
{
    public enum SmsStatusEnum
    {
        New = 0,
        Acked = 1,
        Delivered = 2,
        Failed = 3,
        BufferedPhone = 4,
        BufferedSmsc = 5,
        NonDelivered = 6,
        LostNotification = 7,
        UnknownNotification = 8,
    }
}
