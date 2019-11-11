using System;

namespace BL.Interfaces
{
    public interface IFollowLogic
    {
        Guid getFollowId(Guid userId);
        bool addFollower(Guid followId);
        bool removeFollower(Guid followId);
    }
}
