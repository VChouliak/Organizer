using Organizer.Core.Interfaces.Events.Aggregator;

namespace Organizer.Infrastructure.Aggregator
{
    public class EventAggregator : IEventAggregator
    {
        private readonly List<(Type eventType, Delegate methodToCall)> _eventRegistrations = new List<(Type eventType, Delegate methodCall)>();
        object locker = new object();
        public void Publish<T>(T data)
        {
            List<(Type eventType, Delegate methodToCall)> registrations = null;
            lock (locker)
            {
                registrations = new List<(Type eventType, Delegate methodToCall)>(_eventRegistrations);
            }

            foreach (var registration in registrations)
            {
                if (registration.eventType == typeof(T))
                {
                    ((Action<T>)registration.methodToCall)(data);
                }
            }


        }

        public Subscription Subscribe<T>(Action<T> action)
        {
            if (action != null)
            {
                lock (locker) { _eventRegistrations.Add((typeof(T), action)); }
                return new Subscription(() =>
                {
                    _eventRegistrations.Remove((typeof(T), action));
                });
            }
            return new Subscription(() => { });
        }
    }
}
