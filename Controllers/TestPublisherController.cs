using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestBackgroundProcessing.Model;

namespace TestBackgroundProcessing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestPublisherController : ControllerBase
    {
        private readonly ContributionQueue _contributionQueue;
        private readonly ILogger<TestPublisherController> _logger;
        //private readonly CancellationToken _cancellationToken;

        private readonly IDataContext _dataContext;


        public TestPublisherController(ContributionQueue contributionQueue,
            ILogger<TestPublisherController> logger,
            //IHostApplicationLifetime applicationLifetime)
            IDataContext dataContext)
        {
            _contributionQueue = contributionQueue;
            _logger = logger;
            //_cancellationToken = applicationLifetime.ApplicationStopping;
            _dataContext = dataContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("OK");
        }

        [HttpPost]
        public async Task<TestResource> Post(TestResource data)
        {
            var lastCommit = _dataContext.Commits.OrderByDescending(c => c.Id).FirstOrDefault();
            var commit = new Commit
            {
                Id = lastCommit != null ? lastCommit.Id : 1,
                Timestamp = DateTime.UtcNow
            };
            _dataContext.Commits.Add(commit);

            data.CommitId = commit.Id;
            data.Id = Guid.NewGuid().ToString();
            _dataContext.TestResources.Add(data);

            await _contributionQueue.QueueItemAsync(data.CommitId);

            return data;
        }
    }
}
