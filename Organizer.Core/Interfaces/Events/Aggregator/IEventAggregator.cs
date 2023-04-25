
namespace Organizer.Core.Interfaces.Events.Aggregator
{
    public interface IEventAggregator
    {
        void Publish<T>(T data);
        Subscription Subscribe<T>(Action<T> action);
    }
}