//using System;

//namespace TestBackgroundProcessing
//{
//    public class NotificationEventService : BackgroundService
//    {
//        private readonly ILogger<NotificationEventService> _logger;

//        public NotificationEventService(ISubscriptionNotificationQueue notificationQueue, ILogger<NotificationEventService> logger)
//        {
//            NotificationQueue = notificationQueue;
//            _logger = logger;
//        }

//        public ISubscriptionNotificationQueue NotificationQueue { get; }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            _logger.LogInformation("NotificationBackgroundService is running.");

//            await BackgroundProcessing(stoppingToken);
//        }

//        private async Task BackgroundProcessing(CancellationToken stoppingToken)
//        {
//            while (!stoppingToken.IsCancellationRequested)
//            {
//                var notificationItem = await NotificationQueue.DequeueAsync(stoppingToken);

//                _logger.LogInformation("Queued notification {item} for subscription {subscriptionId} is processing.", notificationItem.Id, notificationItem.SubscriptionId.ToString());

//                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

//                _logger.LogInformation("Queued notification {item} for subscription {subscriptionId} processing completed.", notificationItem.Id, notificationItem.SubscriptionId.ToString());
//            }
//        }

//        public override async Task StopAsync(CancellationToken stoppingToken)
//        {
//            _logger.LogInformation("NotificationBackgroundService is stopping.");

//            await base.StopAsync(stoppingToken);
//        }
//    }
//}
