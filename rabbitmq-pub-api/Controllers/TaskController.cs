using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using rabbitmq_pub_api.Services;
using Task = rabbitmq_pub_api.Models.Task;

namespace rabbitmq_pub_api.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ILogger<TaskController> _logger;
        private IMessageSender _sender;

        public TaskController(ILogger<TaskController> logger, IMessageSender sender)
        {
            _logger = logger;
            _sender = sender;
        }

        /// <summary>
        /// TEst yolo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="sleep"></param>
        /// <param name="count"></param>
        [HttpPost]
        public void Create(string name, string id, int? sleep, int? count)
        {
            int _count = count ?? 100;
            for (int i = 0; i < _count; i++)
            {
                var job = new Task
                {
                    Name = name ?? $"test-{i}",
                    Id = id ?? Guid.NewGuid().ToString(),
                    SleepTime = sleep ?? 5
                };
                _sender.SendMessage(job);
            }

        }
    }
}
