using System;

namespace BO.Models
{
    public class ArticleComment : BaseModel
    {


        public Article Articles { get; set; }

        public Guid ArticleId { get; set; }
        public Comment Comments { get; set; }
        public Guid CommentId { get; set; }
    }
}
