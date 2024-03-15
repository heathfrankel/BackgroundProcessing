using TestBackgroundProcessing.Model;

namespace TestBackgroundProcessing
{
    public interface IDataContext
    {
        IList<Commit> Commits { get; }

        IList<TestResource> TestResources { get; }

        IList<Subscription> Subscriptions { get; }

        IList<NotificationEvent> NotificationEvents { get; }

    }

    public class DataContext : IDataContext
    {
        private readonly List<Commit> _commits = new List<Commit>();
        private readonly List<TestResource> _testResources = new List<TestResource>();
        private readonly List<Subscription> _subscriptions = new List<Subscription>();
        private readonly List<NotificationEvent> _notificationEvents = new List<NotificationEvent>();

        public IList<Commit> Commits
        {
            get { return _commits; }
        }


        public IList<TestResource> TestResources
        {
            get { return _testResources; }
        }


        public IList<Subscription> Subscriptions
        {
            get { return _subscriptions; }
        }


        public IList<NotificationEvent> NotificationEvents
        { 
            get { return _notificationEvents; } 
        }
    }
}
