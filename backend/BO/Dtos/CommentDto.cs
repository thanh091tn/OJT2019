using System;

namespace BO.Dtos
{
    public class CommentDto
    {
        public string Context { get; set; }

        public Guid ArticleId { get; set; }
    }

    public class ShowCommentDto
    {
        public Guid UserId { get; set; }
        public string Img { get; set; }
        public string Context { get; set; }
        public DateTime Date { get; set; }
        public string UserName { get; set; }
        public Guid Id { get; set; }
    }
}