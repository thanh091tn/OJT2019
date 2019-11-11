using System;

namespace BL.Interfaces
{
    public interface ILikeLogic
    {
        bool Like(Guid articleId);
        bool DisLike(Guid articleId);
    }
}
