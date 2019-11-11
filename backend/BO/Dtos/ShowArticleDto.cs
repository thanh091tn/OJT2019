using System.Collections.Generic;

namespace BO.Dtos
{
    public class ShowArticleDto
    {
        public IEnumerable<ArticleGlobalFeedDto> ArticleGlobalFeedDto { get; set; }
        public double MaxPage { get; set; }
    }
}
