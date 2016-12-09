using System.Collections.Generic;
using System.Threading.Tasks;
using DataServices.Models;
using Windows.Foundation;

namespace DataServices
{
    public interface IXmlDataService
    {

        Task<IEnumerable<WiseIdeaPresentModel>> GetThoughtsByAuthorNameAsync(string author);
        Task<IEnumerable<WiseIdeaPresentModel>> GetThoughtsByDayAsync(int day, int month);
        Task AddDeleteFavorite(int id, bool isFavorite);
        Task<IEnumerable<WiseIdeaPresentModel>> GetFavouritesThoughts();
    }
}