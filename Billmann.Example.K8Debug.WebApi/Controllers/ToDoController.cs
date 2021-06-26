using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Billmann.Example.K8Debug.Persistence;
using Billmann.Example.K8Debug.Entities;
using Billmann.Example.K8Debug.Model;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Billmann.Example.K8Debug.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly IDatabase _database;
        private readonly ILogger<ToDoController> _logger;
        

        public ToDoController(IDatabase database, ILogger<ToDoController> logger)
        {
            _database = database;
            _logger = logger;
        }

        [ProducesResponseType(typeof(IList<ToDoItemReadModel>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Get called");
            var items = await _database.All();
            var readModelItems = items.Select(x => ToReadModel(x)).ToList();
            return Ok(items);
        }

        [ProducesResponseType(typeof(ToDoItemReadModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(Guid id)
        {
            _logger.LogInformation("GetItem({Id}) called", id);
            var item = await _database.Find(id);
            if (item == null) {
                return NotFound();
            }
            var readModel  = ToReadModel(item);
            return Ok(item);
        }

        [ProducesResponseType(typeof(ToDoItemReadModel), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> CreateItem(ToDoItemCreateModel createModel)
        {
            _logger.LogInformation("CreateItem() calles");
            if (string.IsNullOrWhiteSpace(createModel.Title) || string.IsNullOrWhiteSpace(createModel.Description))
            {
                return new BadRequestObjectResult("tile and description must contain content");
            }

            var newItem = new ToDoItem 
            {
                Id = Guid.NewGuid(),
                Title = createModel.Title,
                Description = createModel.Description
            };
            await _database.Add(newItem);
            var readModel  = ToReadModel(newItem);
            return Created(nameof(readModel), readModel);
        }

        private static ToDoItemReadModel ToReadModel(ToDoItem entity)
        {
            return new ToDoItemReadModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description
            };
        }
    }
}
