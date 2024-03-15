using System.Threading.Channels;
using TestBackgroundProcessing.Model;

namespace TestBackgroundProcessing
{
    public interface ISubscriptionEventQueue
    {
        ValueTask QueueItemAsync(Subscription item);

        ValueTask<Subscription> DequeueAsync(CancellationToken cancellationToken);
    }

    public class SubscriptionEventQueue : ISubscriptionEventQueue
    {
        private readonly Channel<Subscription> _queue;

        public SubscriptionEventQueue(int capacity)
        {
            // Capacity should be set based on the expected application load and
            // number of concurrent threads accessing the queue.            
            // BoundedChannelFullMode.Wait will cause calls to WriteAsync() to return a task,
            // which completes only when space became available. This leads to backpressure,
            // in case too many publishers/calls start accumulating.
            var options = new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait
            };
            _queue = Channel.CreateBounded<Subscription>(options);
        }

        public async ValueTask QueueItemAsync(Subscription item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            await _queue.Writer.WriteAsync(item);
        }

        public async ValueTask<Subscription> DequeueAsync(CancellationToken cancellationToken)
        {
            var item = await _queue.Reader.ReadAsync(cancellationToken);

            return item;
        }
    }
}
