//using System.Threading.Channels;
//using TestBackgroundProcessing.Model;

//namespace TestBackgroundProcessing
//{
//    public interface ISubscriptionNotificationQueue
//    {
//        ValueTask QueueItemAsync(NotificationEvent item);

//        ValueTask<NotificationEvent> DequeueAsync(CancellationToken cancellationToken);
//    }

//    public class NotificationEventQueue : ISubscriptionNotificationQueue
//    {
//        private readonly Channel<NotificationEvent> _queue;

//        public NotificationEventQueue(int capacity)
//        {
//            // Capacity should be set based on the expected application load and
//            // number of concurrent threads accessing the queue.            
//            // BoundedChannelFullMode.Wait will cause calls to WriteAsync() to return a task,
//            // which completes only when space became available. This leads to backpressure,
//            // in case too many publishers/calls start accumulating.
//            var options = new BoundedChannelOptions(capacity)
//            {
//                FullMode = BoundedChannelFullMode.Wait
//            };
//            _queue = Channel.CreateBounded<NotificationEvent>(options);
//        }

//        public async ValueTask QueueItemAsync(NotificationEvent item)
//        {
//            if (item == null)
//            {
//                throw new ArgumentNullException(nameof(item));
//            }

//            await _queue.Writer.WriteAsync(item);
//        }

//        public async ValueTask<NotificationEvent> DequeueAsync(CancellationToken cancellationToken)
//        {
//            var item = await _queue.Reader.ReadAsync(cancellationToken);

//            return item;
//        }
//    }
//}
