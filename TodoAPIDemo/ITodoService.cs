using System.Threading.Tasks;
using TodoAPIDemo.Entities;

namespace TodoAPIDemo
{
    public interface ITodoService
    {
        Task Patch(string id, Todo item);
    }
}