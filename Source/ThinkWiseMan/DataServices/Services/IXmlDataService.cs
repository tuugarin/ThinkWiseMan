using System.Collections.Generic;
using System.Threading.Tasks;
using DataServices.Models;
using Windows.Foundation;

namespace DataServices
{
    public interface IXmlDataService
    {

        Task<IEnumerable<WiseIdeaModel>> GetThoughtsByAuthorNameAsync(string author);
        Task<IEnumerable<WiseIdeaModel>> GetThoughtsByDayAsync(int day, int month);
        Task AddDeleteFavorite(string id, bool isFavorite);
        Task<IEnumerable<WiseIdeaModel>> GetFavouritesThoughts();
    }
}