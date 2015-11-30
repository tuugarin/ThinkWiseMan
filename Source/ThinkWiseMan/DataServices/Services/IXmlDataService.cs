using System.Collections.Generic;
using System.Threading.Tasks;
using DataServices.Models;
using Windows.Foundation;

namespace DataServices
{
    public interface IXmlDataService
    {

        IAsyncOperation<IEnumerable<WiseIdeaModel>> GetThoughtsByAuthorNameAsync(string author);
        IAsyncOperation<IEnumerable<WiseIdeaModel>> GetThoughtsByDayAsync(int day, int month);
        
    }
}