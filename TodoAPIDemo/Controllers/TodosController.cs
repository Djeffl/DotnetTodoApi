using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TodoAPIDemo.Entities;
using TodoAPIDemo.Errors;

namespace TodoAPIDemo.Controllers
{
    [Produces("application/json")]
    [Route("api/todos")]
    public class TodosController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly TodoContext _context;
        private readonly TodoService _service;

        public TodosController(IConfiguration configuration, ILogger<TodosController> logger, TodoContext context, TodoService service)
        {
            this._configuration = configuration;
            this._logger = logger;
            this._context = context;
            this._service = service;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Todo))]
        public IActionResult Get()
        {
            return Ok(_context.Todos.ToList());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _context.Todos.FindAsync(id));
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Todo todo)
        {
            await _context.Todos.AddAsync(todo);
            await _context.SaveChangesAsync();

            return Created("", todo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var todo = await _context.Todos.FindAsync(id);

            if(todo == null)
            {
                return NotFound();
            }

            _context.Todos.Remove(todo);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(string id, [FromBody]Todo item)
        {
            try
            {
                await _service.Patch(id, item);
                return NoContent();
            }
            catch(ItemNotFoundException)
            {
                return NotFound();
            }
        }

    }
}