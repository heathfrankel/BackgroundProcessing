//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using TestBackgroundProcessing.Model;

//namespace TestBackgroundProcessing.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class TestApiController : ControllerBase
//    {
//        private readonly ILogger<TestApiController> _logger;

//        public TestApiController(ILogger<TestApiController> logger)
//        {
//            _logger = logger;
//        }

//        [HttpGet]
//        public IActionResult Get()
//        {
//            return Ok("OK");
//        }

//        [HttpPost]
//        public TestResource Post(TestResource data)
//        {
//            data.Id = Guid.NewGuid().ToString();

//            return data;
//        }

//    }
//}
