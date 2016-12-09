using DataServices.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices
{
    public class SqlDataService : ISqlDataService
    {
        public async Task<IEnumerable<WiseIdeaPresentModel>> GetThoughtsByDayAsync(int day, int month)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                await context.EnsureSeedData();
                IQueryable<WiseIdeaPresentModel> thoughts =
                         from thought in context.Ideas
                         where thought.Day == day && thought.Month == month
                         select new WiseIdeaPresentModel
                         {
                             Content = thought.Content,
                             Day = thought.Day,
                             Month = thought.Month,
                             Author = thought.Author,
                             IsFavorite = thought.IsFavorite,
                             Id = thought.Id
                         };


                return await thoughts.ToListAsync();
            }
        }

        public async Task<IEnumerable<WiseIdeaPresentModel>> GetFavouritesThoughts()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {

                await context.EnsureSeedData();
                IQueryable<WiseIdeaPresentModel> thoughts =
                            from thought in context.Ideas
                            where thought.IsFavorite == true
                            select new WiseIdeaPresentModel
                            {
                                Content = thought.Content,
                                Day = thought.Day,
                                Month = thought.Month,
                                Author = thought.Author,
                                IsFavorite = thought.IsFavorite,
                                Id = thought.Id
                            };

                return await thoughts.ToListAsync();
            }
        }

        public async Task UpdateFavoritePropery(int id, bool value)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                await context.EnsureSeedData();
                EntityEntry<WiseIdea> entry = context.Ideas.Attach(new WiseIdea { Id = id, IsFavorite = value });
                entry.Property(p => p.IsFavorite).IsModified = true;
                await context.SaveChangesAsync();

            }
        }


    }

}
