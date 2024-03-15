using TestBackgroundProcessing;
using TestBackgroundProcessing.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//builder.Services.AddHostedService<QueuedHostedService>();

//builder.Services.AddSingleton<IBackgroundTaskQueue>(ctx =>
//{
//    if (!int.TryParse(builder.Configuration["QueueCapacity"], out var queueCapacity))
//        queueCapacity = 100;
//    return new BackgroundTaskQueue(queueCapacity);
//});

builder.Services.AddHostedService<SubscriptionService>();
builder.Services.AddSingleton<ContributionQueue>(ctx =>
{
    if (!int.TryParse(builder.Configuration["QueueCapacity"], out var queueCapacity))
        queueCapacity = 100;
    return new ContributionQueue(queueCapacity);
});

//builder.Services.AddHostedService<NotificationEventService>();
//builder.Services.AddSingleton<ISubscriptionNotificationQueue>(ctx =>
//{
//    if (!int.TryParse(builder.Configuration["QueueCapacity"], out var queueCapacity))
//        queueCapacity = 100;
//    return new NotificationEventQueue(queueCapacity);
//});

builder.Services.AddSingleton<ISubscriptionEventQueue>(ctx =>
{
    if (!int.TryParse(builder.Configuration["QueueCapacity"], out var queueCapacity))
        queueCapacity = 100;
    return new SubscriptionEventQueue(queueCapacity);
});

builder.Services.AddSingleton<DataContext>(ctx => {
    var dc = new DataContext();

    dc.Subscriptions.Add (new Subscription { 
        Id = Guid.NewGuid(), Status = "active", 
        TopicUrl = QuestionnaireResponseCompletedSubscriptionProcessor.SubscriptionTopic, 
        ChannelType = "rest-hook",
        ChannelEndpoint = "https://webhook.site/927a4459-2cea-4200-8c9d-fed2ea6ca955",
        ChannelPayload = "application/fhir+json",
        ChannelPayloadContent = "id-only",
        ChannelHeader = "[\"Authorization: Bearer secret-token-abc-123\"]"
    });

    dc.Subscriptions.Add(new Subscription
    {
        Id = Guid.NewGuid(),
        Status = "active",
        TopicUrl = QuestionnaireResponseCompletedSubscriptionProcessor.SubscriptionTopic,
        ChannelType = "rest-hook",
        ChannelEndpoint = "https://webhook.site/616aef2f-b406-41fe-90a5-dddc11c7f7de",
        ChannelPayload = "application/fhir+json",
        ChannelPayloadContent = "id-only",
        ChannelHeader = "[\"Authorization: Bearer secret-token-abc-123\"]"
    });

    return dc;
});

builder.Services.AddSingleton<SubscriptionTopicProcessorFactory>(new SubscriptionTopicProcessorFactory());

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
