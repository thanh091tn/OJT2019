using BO.Dtos;
using System;
using System.Collections.Generic;

namespace BL.Interfaces
{
    public interface ICommentLogic
    {
        bool PostComment(CommentDto commentDto);
        bool DeleteComment(Guid commentId);
        IEnumerable<ShowCommentDto> GetComments(Guid articleId);
    }
}
