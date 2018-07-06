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

namespace TodoAPIDemo.Controllers
{
    [Produces("application/json")]
    [Route("api/Todo")]
    public class TodoController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public TodoController(IConfiguration configuration, ILogger<TodoController> logger)
        {
            this._configuration = configuration;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Todo>> Get()
        {
            var todos = new List<Todo>();

            string connString = _configuration.GetConnectionString("SqlConnection");
           _logger.LogDebug(connString);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                SqlCommand command = new SqlCommand("SELECT * FROM Todos", conn);
                
                using(SqlDataReader reader = command.ExecuteReader())
                {
                    
                    while(await reader.ReadAsync())
                    {
                        todos.Add(new Todo { Name = reader[0].ToString() });
                    }
                }
            }

            return todos;
        }


    }
}