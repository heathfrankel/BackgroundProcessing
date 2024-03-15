﻿using System.Threading.Channels;

namespace TestBackgroundProcessing
{
    public interface IContributionQueue<T>
    {
        ValueTask QueueItemAsync(T item);

        ValueTask<T> DequeueAsync(CancellationToken cancellationToken);
    }

    public class ContributionQueue : IContributionQueue<long>
    {
        private readonly Channel<long> _queue;

        public ContributionQueue(int capacity)
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
            _queue = Channel.CreateBounded<long>(options);
        }

        public async ValueTask QueueItemAsync(long commitId)
        {
            //if (item == null)
            //{
            //    throw new ArgumentNullException(nameof(item));
            //}
            if (commitId < 1) {
                throw new ArgumentException(nameof(commitId));
            }

            await _queue.Writer.WriteAsync(commitId);
        }

        public async ValueTask<long> DequeueAsync(CancellationToken cancellationToken)
        {
            var item = await _queue.Reader.ReadAsync(cancellationToken);

            return item;
        }
    }
}
