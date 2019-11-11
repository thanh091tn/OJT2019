using BL.Commons;
using BL.Interfaces;
using BO.Models;
using DAL.Repository;
using DAL.Repository.UnitOfWorks;
using System;
using System.Linq;
namespace BL
{


    public class FollowLogic : IFollowLogic
    {
        private readonly UserHelper _userHelper;
        private readonly IUnitOfWork _uow;

        public FollowLogic( UserHelper userHelper, IUnitOfWork uow)
        {
            _userHelper = userHelper;
            _uow = uow;
        }
        //Get FollowID of current userId
        public Guid getFollowId(Guid userId)
        {

            var result = _uow.GetRepository<Follow>().GetAll().First(c => c.FollowerId == userId);
            return result.Id;
        }

        public UserFollow GetFollow(Guid followId)
        {
            var userId = _userHelper.GetUserId();
            var rs =  _uow.GetRepository<UserFollow>().GetAll().FirstOrDefault(c => c.UserId == userId && c.FollowId == followId);
            if(rs == null)
            {
                return new UserFollow()
                {
                    FollowId = followId,
                    UserId = userId
                };
            }
            return rs;
        }

        //Add follower
        public bool addFollower(Guid followId)
        {
            _uow.GetRepository<UserFollow>().Insert(GetFollow(followId));
            
            return true;
        }

        //Remove follower
        public bool removeFollower(Guid followId)
        {
            _uow.GetRepository<UserFollow>().Delete(GetFollow(followId));

            
            return true;
        }
    }
}
