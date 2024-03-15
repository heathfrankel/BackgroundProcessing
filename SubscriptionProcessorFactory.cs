namespace TestBackgroundProcessing
{
    public interface ISubscriptionProcessor
    {
        IList<string> MatchedResources(long commitId);
    }

    public class QuestionnaireResponseCompletedSubscriptionProcessor : ISubscriptionProcessor
    {
        public static string SubscriptionTopic = "http://test.com/SubscriptionTopic/questionnaireresponse-completed";

        public IList<string> MatchedResources(long commitId)
        {
            throw new NotImplementedException();
        }
    }


    public class SubscriptionTopicProcessorFactory
    {
        private readonly Dictionary<string, ISubscriptionProcessor> _subscriptionProcessors;

        public SubscriptionTopicProcessorFactory()
        {
            _subscriptionProcessors = new Dictionary<string, ISubscriptionProcessor>();

            _subscriptionProcessors.Add(QuestionnaireResponseCompletedSubscriptionProcessor.SubscriptionTopic, new QuestionnaireResponseCompletedSubscriptionProcessor());
        }

        public ISubscriptionProcessor Create(string canonicalUrl)
        {
            if (_subscriptionProcessors.ContainsKey(canonicalUrl))
                return _subscriptionProcessors[canonicalUrl];
            //else
            //    return null;

            throw new ArgumentException("SubscriptionTopic processor not for " + canonicalUrl);
        }
    }

    //public class SubscriptionTopic
    //{
    //    public static string QuestionnaireResponse_Completed = "http://test.com/SubscriptionTopic/questionnaireresponse-completed";

    //}
}
