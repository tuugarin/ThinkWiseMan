using System.Collections.Generic;
using System.Threading.Tasks;
using ThinkWiseMan.Models;

namespace ThinkWiseMan.Services
{
    public interface IXmlDataService
    {
        Task<IEnumerable<WiseIdeaModel>> GetThoughtsByAuthorName(string author);
        Task<IEnumerable<WiseIdeaModel>> GetThoughtsByDay(int day, int month);
    }
}