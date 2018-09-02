using System.Threading.Tasks;
using TodoAPIDemo.Core.Models;

namespace TodoAPIDemo.Core.Interfaces
{
    public interface ITodoService
    {
        Task Patch(string id, Todo item);
    }
}