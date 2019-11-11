using BL.Commons;
using BL.Interfaces;
using BO.Models;
using DAL.Repository;
using DAL.Repository.UnitOfWorks;
using System;
using System.Linq;

namespace BL
{

    public class LikeLogic : ILikeLogic
    {

        private readonly UserHelper _userHelper;
        private readonly IUnitOfWork _uow;

        public LikeLogic( UserHelper userHelper, IUnitOfWork uow)
        {
   
            _userHelper = userHelper;
            _uow = uow;
        }

        //Get like
        public Like GetLike(Guid articleId)
        {
            var userId = _userHelper.GetUserId();
            var rs = _uow.GetRepository<Like>().GetAll().FirstOrDefault(like => like.ArticleId == articleId && like.UserId == userId);
            if(rs == null)
            {
                return new Like()
                {
                    ArticleId = articleId,
                    UserId = userId
                };
            }
            return rs;
        }

        //Add like
        public bool Like(Guid articleId)
        {

            _uow.GetRepository<Like>().Insert(GetLike(articleId));
            var item = _uow.GetRepository<Article>().GetAll().FirstOrDefault(c => c.Id == articleId);
            if (item != null)
            {
                item.Like = ++item.Like;
                _uow.SaveChange();
                return true;
            }
            return false;
        }

        //Remove like
        public bool DisLike(Guid articleId)
        {
            _uow.GetRepository<Like>().Delete(GetLike(articleId));

            var item = _uow.GetRepository<Article>().GetAll().FirstOrDefault(c => c.Id == articleId);
            if (item != null)
            {
                item.Like = --item.Like;
                _uow.SaveChange();
                return true;
            }
            return false;
        }
    }
}