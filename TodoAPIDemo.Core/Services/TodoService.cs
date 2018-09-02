using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TodoAPIDemo.Core.Exceptions;
using TodoAPIDemo.Core.Interfaces;
using TodoAPIDemo.Core.Models;

namespace TodoAPIDemo.Core.Services
{
    class TodoService : ITodoService
    {
        private readonly TodoContext _context;

        public TodoService(TodoContext context)
        {
            this._context = context;
        }

        public async Task Patch(string id, Todo item)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                throw new ItemNotFoundException();
            }

            todo.Name = item.Name;
            todo.IsCompleted = item.IsCompleted;

            _context.Todos.Update(todo);
            await _context.SaveChangesAsync();
        }
    }
}
