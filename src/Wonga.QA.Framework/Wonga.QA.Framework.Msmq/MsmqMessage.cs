namespace Wonga.QA.Framework.Msmq
{
    public abstract class MsmqMessage
    {
    }

    public abstract class MsmqMessage<T> where T : MsmqMessage<T>
    {

    }
}
