using System.Collections.Generic;
using System.Threading.Tasks;
using DataServices.Models;

namespace DataServices
{
    public interface ISqlDataService
    {
        Task<IEnumerable<WiseIdeaPresentModel>> GetFavouritesThoughts();
        Task<IEnumerable<WiseIdeaPresentModel>> GetThoughtsByDayAsync(int day, int month);
        Task UpdateFavoritePropery(int id, bool value);
        //Task Migrate();
    }
}