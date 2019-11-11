using System.Collections.Generic;

namespace BO.Models
{
    public class Tag : BaseModel
    {

        public string TagName { get; set; }
        public int TagCount { get; set; }

        public ICollection<ArticleTag> ArticleTags { get; set; }

    }
}
