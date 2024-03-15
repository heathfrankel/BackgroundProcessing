namespace TestBackgroundProcessing.Model
{
    public class NotificationEvent
    {
        public long Id { get; set; }
        public Guid SubscriptionId { get; set; }

        public long EventNumber { get; set; }

        public DateTime? Timestamp { get; set; }

        public string? FocusReference { get; set; }

        public DateTime Created { get; set; }

    }
}
