namespace TestBackgroundProcessing.Model
{
    public class Commit
    {
        public long Id { get; set; }
        public DateTime Timestamp { get; set; }

        public bool EventsProcessed { get; set; }
    }
}
