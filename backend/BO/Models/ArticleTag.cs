using System;

namespace BO.Models
{
    public class ArticleTag : BaseModel
    {

        public Tag Tags { get; set; }
        public Guid TagId { get; set; }
        public Article Articles { get; set; }
        public Guid ArticleId { get; set; }
    }
}
