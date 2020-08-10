using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ArticleApp.Models
{
    public class Article
    {
        public int Id { get; set; }

        [DisplayName("Title of the Article")]
        public string Title { get; set; }

        public DateTime Date { get; set; }
        public string Image { get; set; }
        public string Topic { get; set; }
        [ForeignKey("Category")]
        public int CaregoryId { get; set; }
        public Category Category { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }

    }
}
