using DataServices.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServices
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<WiseIdea> Ideas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            string sqlpath = "WiseIdeas.db"; 
            optionsBuilder.UseSqlite(@"Filename="+sqlpath);
            base.OnConfiguring(optionsBuilder);
            
        }

    }
}
