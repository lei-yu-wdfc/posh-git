namespace Wonga.QA.Framework.Msmq.Enums.PpsProvider.InternalMessages.SagaMessages
{
    public enum ProcessStatusEnum
    {
        New = 0,
        Processing = 1,
        Created = 2,
        Activated = 3,
        Disactivated = 4,
        Failed = 5,
        Updated = 6,
    }
}
