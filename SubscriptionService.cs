using Microsoft.AspNetCore.Http.HttpResults;
using System;
using TestBackgroundProcessing.Model;

namespace TestBackgroundProcessing
{
    public class SubscriptionService : BackgroundService
    {
        private readonly ILogger<SubscriptionService> _logger;


        //private readonly Guid subscription1 = Guid.NewGuid();
        //private readonly Guid subscription2 = Guid.NewGuid();

        //private readonly List<Guid> subscriptions;

        //private long notificationId = 1;
        //private long subscription1Count = 1;
        //private long subscription2Count = 1;

        private readonly DataContext _dataContext;
        private readonly SubscriptionTopicProcessorFactory _subscriptionTopicProcessorFactory;

        public SubscriptionService(ContributionQueue contributionQueue, 
            ILogger<SubscriptionService> logger, 
            ISubscriptionEventQueue subscriptionEventQueue, 
            SubscriptionTopicProcessorFactory subscriptionTopicProcessorFactory,
            DataContext dataContext)
        {
            ContributionQueue = contributionQueue;
            SubscriptionEventQueue = subscriptionEventQueue;
            _logger = logger;

            //subscriptions = new List<Guid>{ subscription1, subscription2 };
            _dataContext = dataContext;
            _subscriptionTopicProcessorFactory = subscriptionTopicProcessorFactory;
        }

        public ContributionQueue ContributionQueue { get; }

        public ISubscriptionEventQueue SubscriptionEventQueue { get; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ContributionBackgroundService is running.");

            await BackgroundProcessing(stoppingToken);

            await SubscriptionEventProcessing(stoppingToken);
        }

        private async Task BackgroundProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var contributionItem = await ContributionQueue.DequeueAsync(stoppingToken);

                _logger.LogInformation("Queued contribution {item} subscription processing started.", contributionItem);


                var activeSubscriptions = _dataContext.Subscriptions.Where(s => s.Status == "active").ToList();

                _dataContext.Commits.Where(c => !c.EventsProcessed && c.Id <= contributionItem).ToList().ForEach(c =>
                {
                    var triggeredSubscriptions = new List<Subscription>();

                    activeSubscriptions.ForEach(s => {
                        var processor = _subscriptionTopicProcessorFactory.Create(s.TopicUrl);
                        var matchedResources = processor.MatchedResources(c.Id).ToList();

                        if (matchedResources.Any())
                        {
                            var lastSubscriptionEvent = _dataContext.NotificationEvents.Where(n => n.SubscriptionId == s.Id).OrderByDescending(n => n.EventNumber).FirstOrDefault();
                            var lastNotification = _dataContext.NotificationEvents.OrderByDescending(n => n.Id).FirstOrDefault();

                            matchedResources.ForEach(r =>
                            {
                                var notificationEvent = new NotificationEvent
                                {
                                    Id = lastNotification == null ? 1 : lastNotification.Id + 1,
                                    EventNumber = lastSubscriptionEvent == null ? 1 : lastSubscriptionEvent.EventNumber + 1,
                                    Timestamp = c.Timestamp,
                                    SubscriptionId = s.Id,
                                    FocusReference = r,
                                    Created = DateTime.UtcNow
                                };

                                _dataContext.NotificationEvents.Add(notificationEvent);
                            });
                        }
                        triggeredSubscriptions.Add(s);
                    });

                    c.EventsProcessed = true;

                    triggeredSubscriptions.ForEach(async s =>
                    {
                        await SubscriptionEventQueue.QueueItemAsync(s);
                    });
                });

                //await Parallel.ForEachAsync(subscriptions,
                //    new ParallelOptions { MaxDegreeOfParallelism = 2 }, async (id, _) =>
                //    {

                //        var notificatication1 = new NotificationEvent
                //        {
                //            Timestamp = DateTime.UtcNow,
                //            Id = notificationId++,
                //            SubscriptionId = id,
                //            //EventNumber = subscription1Count++,
                //            FocusReference = contributionItem,
                //            Created = DateTime.UtcNow
                //        };

                //        await NotificationQueue.QueueItemAsync(notificatication1);
                //    });

                ////var notificatication1 = new SubscriptionNotification
                ////{
                ////    Timestamp = DateTime.UtcNow,
                ////    Id = notificationId++,
                ////    SubscriptionId = subscription1,
                ////    EventNumber = subscription1Count++,
                ////    FocusReference = contributionItem,
                ////    Created = DateTime.UtcNow
                ////};

                ////await NotificationQueue.QueueItemAsync(notificatication1);

                ////var notificatication2 = new SubscriptionNotification
                ////{
                ////    Timestamp = notificatication1.Timestamp,
                ////    Id = notificationId++,
                ////    SubscriptionId = subscription2,
                ////    EventNumber = subscription2Count++,
                ////    FocusReference = contributionItem,
                ////    Created = DateTime.UtcNow
                ////};

                ////await NotificationQueue.QueueItemAsync(notificatication2);

                _logger.LogInformation("Queued contribution {item} subscription processing completed.", contributionItem);
            }
        }

        private async Task SubscriptionEventProcessing(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var subscriptionEventItem = await SubscriptionEventQueue.DequeueAsync(stoppingToken);

                _logger.LogInformation("Queued subscription {subscriptionId} event is processing.", subscriptionEventItem.Id.ToString());

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

                _logger.LogInformation("Queued subscription {subscriptionId} event processing completed.", subscriptionEventItem.Id.ToString());
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ContributionBackgroundService is stopping.");

            await base.StopAsync(stoppingToken);
        }
    }
}
