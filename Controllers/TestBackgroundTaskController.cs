//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using TestBackgroundProcessing.Model;

//namespace TestBackgroundProcessing.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TestBackgroundTaskController : ControllerBase
//    {
//        private readonly IBackgroundTaskQueue _taskQueue;
//        private readonly ILogger<TestBackgroundTaskController> _logger;
//        private readonly CancellationToken _cancellationToken;

//        public TestBackgroundTaskController(IBackgroundTaskQueue taskQueue,
//            ILogger<TestBackgroundTaskController> logger,
//            IHostApplicationLifetime applicationLifetime)
//        {
//            _taskQueue = taskQueue;
//            _logger = logger;
//            _cancellationToken = applicationLifetime.ApplicationStopping;
//        }

//        [HttpGet]
//        public IActionResult Get()
//        {
//            return Ok("OK");
//        }

//        [HttpPost]
//        public async Task<TestResource> Post(TestResource data)
//        {
//            data.Id = Guid.NewGuid().ToString();

//            await _taskQueue.QueueBackgroundWorkItemAsync(BuildWorkItem);

//            return data;
//        }


//        private async ValueTask BuildWorkItem(CancellationToken token)
//        {
//            // Simulate three 5-second tasks to complete
//            // for each enqueued work item

//            int delayLoop = 0;
//            var guid = Guid.NewGuid().ToString();

//            _logger.LogInformation("Queued Background Task {Guid} is starting.", guid);

//            while (!token.IsCancellationRequested && delayLoop < 3)
//            {
//                try
//                {
//                    await Task.Delay(TimeSpan.FromSeconds(5), token);
//                }
//                catch (OperationCanceledException)
//                {
//                    // Prevent throwing if the Delay is cancelled
//                }

//                delayLoop++;

//                _logger.LogInformation("Queued Background Task {Guid} is running. "
//                                       + "{DelayLoop}/3", guid, delayLoop);
//            }

//            if (delayLoop == 3)
//            {
//                _logger.LogInformation("Queued Background Task {Guid} is complete.", guid);
//            }
//            else
//            {
//                _logger.LogInformation("Queued Background Task {Guid} was cancelled.", guid);
//            }
//        }
//    }
//}
