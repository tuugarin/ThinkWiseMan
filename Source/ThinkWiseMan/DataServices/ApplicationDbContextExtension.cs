using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace DataServices
{
    static class ApplicationDbContextExtension
    {

        public static async Task EnsureSeedData(this ApplicationDbContext context)
        {
            await context.Database.MigrateAsync();
            if (!context.Ideas.Any())
            {

                var thoughts = await XmlDataService.GetAllThoughts();
                await context.Ideas.AddRangeAsync(thoughts);
                await context.SaveChangesAsync();
            }
        }
    }
}
