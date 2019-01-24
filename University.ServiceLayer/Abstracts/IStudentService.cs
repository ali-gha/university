using System.Threading.Tasks;
using University.BusinessLayer.Models;

namespace University.ServiceLayer.Abstracts
{
    public interface IStudentService : IGenericService<Student>
    {
        Task<bool> FindStudentByName(string name);
    }
}