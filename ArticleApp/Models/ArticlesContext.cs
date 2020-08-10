using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArticleApp.Models
{
    public class ArticlesContext : DbContext
    {
        public ArticlesContext(DbContextOptions<ArticlesContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<ContactUs> Contacts { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
    }
}
