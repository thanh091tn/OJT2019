using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BO.Models
{
    public class Article : BaseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public int Like { get; set; }



        public DateTime DateTime { get; set; }

        public User User { get; set; }
        [ForeignKey("User")]
        public Guid CreatedByUid { get; set; }

        public ICollection<Like> Likes { get; set; }
        public ICollection<ArticleTag> ArticleTags { get; set; }

    }
}
