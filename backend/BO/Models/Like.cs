using System;

namespace BO.Models
{
    public class Like : BaseModel
    {
        public User Users { get; set; }
        public Guid UserId { get; set; }

        public Article Articles { get; set; }
        public Guid ArticleId { get; set; }

    }
}
