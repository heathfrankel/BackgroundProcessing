namespace TestBackgroundProcessing.Model
{
    public class Subscription
    {
        public Guid Id { get; set; }
        public string Status { get; set; }

        public string TopicUrl { get; set; }

        public string[] FilterCriteria { get; set; }

        public string ChannelType { get; set; }

        public string ChannelEndpoint { get; set; }

        public string ChannelPayload { get; set; }

        public string ChannelPayloadContent { get; set; }

        public string ChannelHeader { get; set; }
    }
}
