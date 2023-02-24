namespace Organizer.Core.Interfaces.Events.Aggregator
{
    public class Subscription : IDisposable
    {
        private readonly Action _removeMethod;
        public Subscription(Action removeMethod)
        {
            _removeMethod = removeMethod;
        }       

        public void Dispose()
        {
            if (_removeMethod != null)
            {
                _removeMethod();
            }
        }
    }
}
