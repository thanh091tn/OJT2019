using BL.Commons;
using BL.Interfaces;
using BO.Dtos;
using BO.Models;
using DAL.Repository;
using DAL.Repository.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
namespace BL
{


    public class CommentLogic : ICommentLogic
    {
 
        private readonly UserHelper _userHelper;
        private readonly IUnitOfWork _uow;

        public CommentLogic( UserHelper userHelper, IUnitOfWork uow)
        {
   
            _userHelper = userHelper;
            _uow = uow;
        }

        //Add new comment
        public bool PostComment(CommentDto commentDto)
        {
            var currentuserId = _userHelper.GetUserId();

            var comment = new Comment()
            {
                Id = new Guid(),
                Date = DateTime.Now,
                Context = commentDto.Context,
                UserId = currentuserId

            };
            _uow.GetRepository<Comment>().Insert(comment);


            _uow.GetRepository<ArticleComment>().Insert(new ArticleComment()
            {
                ArticleId = commentDto.ArticleId,
                CommentId = comment.Id
            });

            _uow.SaveChange();

            return true;
        }

        //Delete existed comment
        public bool DeleteComment(Guid commentId)
        {
            var result = _uow.GetRepository<Comment>().GetAll().FirstOrDefault(c => c.Id == commentId);
            _uow.GetRepository<Comment>().Delete(result);
            _uow.SaveChange();
            return true;
        }

        //Get list commentId
        public List<Guid> GetCommentIds(Guid articleId)
        {
            var result = _uow.GetRepository<ArticleComment>().GetAll()
                .Include(c => c.Comments)
                .Where(c => c.ArticleId == articleId)
                .Select(c => new ArticleComment()
                {
                    CommentId = c.CommentId
                }); ;
            List<Guid> commentIdList = new List<Guid>();
            foreach (var comment in result)
            {
                commentIdList.Add(comment.CommentId);
            }
            return commentIdList;
        }

        //Get comment by articleId
        public IEnumerable<ShowCommentDto> GetComments(Guid articleId)
        {
            List<Guid> listCommentId = GetCommentIds(articleId);
            return _uow.GetRepository<Comment>().GetAll()
                .Include(comment => comment.Users)
                .Where(c => listCommentId.Contains(c.Id))
                .Select(c => new ShowCommentDto()
                {
                    UserName = c.Users.UserName,
                    Date = c.Date,
                    Context = c.Context,
                    Img = c.Users.Img,
                    UserId = c.UserId,
                    Id = c.Id

                });
        }
    }
}