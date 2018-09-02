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
    [Route("api/todos")]
    public class TodosController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public TodosController(IConfiguration configuration, ILogger<TodosController> logger)
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
                        todos.Add(new Todo { Id = reader[0].ToString(), Name = reader[1].ToString() });
                    }
                }
            }

            return todos;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<Todo> Get(string id)
        {
            return (await Get()).Where(todo => todo.Id == id).FirstOrDefault();
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody]Todo todo)
        {
            string connString = _configuration.GetConnectionString("SqlConnection");
            _logger.LogDebug(connString);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                SqlCommand command = conn.CreateCommand();

                SqlTransaction transaction = conn.BeginTransaction("Insert Todo");
                Guid id = Guid.NewGuid();

                command.Connection = conn;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = "INSERT INTO Todos(Id, Name) VALUES ('" + id + "', '" + todo.Name + "')";
                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception exRollback)
                    {
                        _logger.LogError(exRollback.Message);
                    }
                }
            }
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            var todos = new List<Todo>();

            string connString = _configuration.GetConnectionString("SqlConnection");
            _logger.LogDebug(connString);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                SqlCommand command = conn.CreateCommand();

                SqlTransaction transaction = conn.BeginTransaction("PatchTodo");

                command.Connection = conn;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = "DELETE FROM Todos WHERE Id = '" + id + "'";
                    await command.ExecuteNonQueryAsync();

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception exRollback)
                    {
                        _logger.LogError(exRollback.Message);
                    }
                }
            }
        }

        [HttpPatch("{id}")]
        public async Task Patch(string id, [FromBody]Todo todo)
        {
            var todos = new List<Todo>();

            string connString = _configuration.GetConnectionString("SqlConnection");
            _logger.LogDebug(connString);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();

                SqlCommand command = conn.CreateCommand();

                SqlTransaction transaction = conn.BeginTransaction("PatchTodo");

                command.Connection = conn;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = "UPDATE Todos SET Name= '" + todo.Name + "' WHERE Id = '" + id + "'";
                    await command.ExecuteNonQueryAsync();

                    transaction.Commit();
                } catch(Exception ex)
                {
                    _logger.LogError(ex.Message);
                    try
                    {
                        transaction.Rollback();
                    } catch(Exception exRollback)
                    {
                        _logger.LogError(exRollback.Message);
                    }
                }
            }
        }

    }
}