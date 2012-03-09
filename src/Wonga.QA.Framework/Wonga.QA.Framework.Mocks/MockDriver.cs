namespace Wonga.QA.Framework.Mocks
{
    public class MockDriver
    {
        private Scotia _scotia;

        public Scotia Scotia
        {
            get { return _scotia ?? (_scotia = new Scotia()); }
        }
    }
}