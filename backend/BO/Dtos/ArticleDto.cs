using System;
using System.Collections.Generic;

namespace BO.Dtos
{
    public class ArticleDto
    {
        public string Title { get; set; }
    }

    public class ArticleDetailsDto : ArticleDto
    {
        public string Img { get; set; }
        public DateTime DateTime { get; set; }
        public string Username { get; set; }
        public int Like { get; set; }
        public string Description { get; set; }

        public string Content { get; set; }
        public bool isLike { get; set; }
        public IEnumerable<TagDto> TagId { get; set; }
    }

    public class ArticleEditDetailsDto : ArticleDto
    {
        public string Description { get; set; }
        public string Content { get; set; }
        public IEnumerable<TagDto> TagId { get; set; }
    }

    public class ArticleCreateNewDto : ArticleDto
    {
        public string Description { get; set; }
        public string Content { get; set; }
        public IEnumerable<string> TagName { get; set; }
    }

    public class ArticleGlobalFeedDto : ArticleDto
    {
        public Guid ArticleId { get; set; }
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Username { get; set; }
        public string Img { get; set; }
        public string Description { get; set; }
        public IEnumerable<TagDto> TagId { get; set; }
        public int Like { get; set; }
        public bool isLike { get; set; }
    }

    public class ArticleEditDto : ArticleDto
    {
        public string Description { get; set; }
        public string Content { get; set; }
        public IEnumerable<string> TagName { get; set; }
    }
}
