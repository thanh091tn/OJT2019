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
    public class ArticleLogic : IArticleLogic
    {
        private readonly UserHelper _userHelper;
        private readonly IUnitOfWork _uow;

        public ArticleLogic(UserHelper userHelper, IUnitOfWork uow)
        {
            _userHelper = userHelper;
            _uow = uow;
        }

        public double GetMaxPage()
        {
            double maxPage = _uow.GetRepository<Article>().GetAll().Count();

            var numberPage = Math.Ceiling(maxPage / 10);

            if (numberPage > 50)
            {
                numberPage = 50;
            }

            return numberPage;
        }

        //Get articles order by date created 
        public ShowArticleDto GetGlobalArticles(int currentPage, int pageRange)
        {
            var currentUserId = _userHelper.GetUserId();

            double maxPage = GetMaxPage();

            List<ArticleGlobalFeedDto> GlobalArticle = _uow.GetRepository<Article>().GetAll()
                .Include(c => c.User)
                .Include(c => c.ArticleTags)
                .OrderByDescending(c => c.DateTime)
                .Skip((currentPage - 1) * pageRange).Take(pageRange)
                .Select(c => new ArticleGlobalFeedDto()
                {
                    ArticleId = c.Id,
                    Id = c.User.Id,
                    Img = c.User.Img,
                    Username = c.User.UserName,
                    DateTime = c.DateTime,
                    Title = c.Title,
                    Description = c.Description,
                    Like = c.Like,
                    TagId = _uow.GetRepository<ArticleTag>().GetAll()
                    .Include(tag => tag.Tags)
                    .Where(a => a.ArticleId == c.Id)
                    .Select(b => new TagDto()
                    {
                        TagId = b.Tags.Id,
                        TagName = b.Tags.TagName
                    }),
                    isLike = false
                }).ToList();

            List<bool> isLikeList = new List<bool>();

            foreach (var article in GlobalArticle)
            {
                isLikeList.Add(_uow.GetRepository<Like>().GetAll()
                    .Any(like => like.UserId == currentUserId && like.ArticleId == article.ArticleId));
            }

            List<ArticleGlobalFeedDto> globalFeedDtosList = new List<ArticleGlobalFeedDto>();

            for (int i = 0; i < GlobalArticle.Count(); i++)
            {
                globalFeedDtosList.Add(new ArticleGlobalFeedDto()
                {
                    ArticleId = GlobalArticle.ElementAt(i).ArticleId,
                    Id = GlobalArticle.ElementAt(i).Id,
                    Username = GlobalArticle.ElementAt(i).Username,
                    Img = GlobalArticle.ElementAt(i).Img,
                    DateTime = GlobalArticle.ElementAt(i).DateTime,
                    Title = GlobalArticle.ElementAt(i).Title,
                    Description = GlobalArticle.ElementAt(i).Description,
                    Like = GlobalArticle.ElementAt(i).Like,
                    TagId = GlobalArticle.ElementAt(i).TagId,
                    isLike = isLikeList.ElementAt(i)
                });
            }

            return new ShowArticleDto()
            {
                ArticleGlobalFeedDto = globalFeedDtosList,
                MaxPage = maxPage
            };
        }

        //Get articles by tagId (order by articles date created) 
        public ShowArticleDto GetGlobalArticlesByTagId(Guid tagId, int currentPage, int pageRange)
        {
            var currentUserId = _userHelper.GetUserId();
            double maxPage = GetMaxPage();

            var result = _uow.GetRepository<ArticleTag>().GetAll()
                .Where(c => c.TagId == tagId)
                .Select(c => new ArticleTag()
                {
                    ArticleId = c.ArticleId
                });

            List<Guid> temp = new List<Guid>();

            foreach (var articleTag in result)
            {
                temp.Add(articleTag.ArticleId);
            }

            List<ArticleGlobalFeedDto> ArticleList = _uow.GetRepository<Article>().GetAll()
                .Include(c => c.User)
                .Include(c => c.ArticleTags)
                .Where(c => temp.Contains(c.Id))
                .OrderByDescending(c => c.DateTime)
                .Skip((currentPage - 1) * pageRange).Take(pageRange)
                .Select(c =>
                new ArticleGlobalFeedDto()
                {
                    ArticleId = c.Id,
                    Id = c.User.Id,
                    Username = c.User.UserName,
                    Img = c.User.Img,
                    DateTime = c.DateTime,
                    Title = c.Title,
                    Description = c.Description,
                    Like = c.Like,
                    TagId = _uow.GetRepository<ArticleTag>().GetAll()
                    .Include(tag => tag.Tags)
                    .Where(a => a.ArticleId == c.Id)
                    .Select(b => new TagDto()
                    {
                        TagId = b.Tags.Id,
                        TagName = b.Tags.TagName
                    }),
                    isLike = false
                }).ToList();

            List<bool> isLikeList = new List<bool>();

            foreach (var article in ArticleList)
            {
                isLikeList.Add(_uow.GetRepository<Like>().GetAll()
                    .Any(like => like.UserId == currentUserId && like.ArticleId == article.ArticleId));
            }

            List<ArticleGlobalFeedDto> globalFeedDtosList = new List<ArticleGlobalFeedDto>();

            for (int i = 0; i < ArticleList.Count(); i++)
            {
                globalFeedDtosList.Add(new ArticleGlobalFeedDto()
                {
                    ArticleId = ArticleList.ElementAt(i).ArticleId,
                    Id = ArticleList.ElementAt(i).Id,
                    Username = ArticleList.ElementAt(i).Username,
                    Img = ArticleList.ElementAt(i).Img,
                    DateTime = ArticleList.ElementAt(i).DateTime,
                    Title = ArticleList.ElementAt(i).Title,
                    Description = ArticleList.ElementAt(i).Description,
                    Like = ArticleList.ElementAt(i).Like,
                    TagId = ArticleList.ElementAt(i).TagId,
                    isLike = isLikeList.ElementAt(i)
                });
            }

            return new ShowArticleDto()
            {
                ArticleGlobalFeedDto = globalFeedDtosList,
                MaxPage = maxPage
            };
        }


        //Get article details by articleId
        public IQueryable<ArticleDto> GetArticleDetails(Guid articleId)
        {
            var currentUserId = _userHelper.GetUserId();

            return _uow.GetRepository<Article>().GetAll()
                .Include(c => c.User)
                .Where(a => a.Id == articleId)
                .Select(c => new ArticleDetailsDto()
                {
                    Username = c.User.UserName,
                    Img = c.User.Img,
                    DateTime = c.DateTime,
                    Title = c.Title,
                    Content = c.Content,
                    Description = c.Description,
                    Like = c.Like,
                    isLike = _uow.GetRepository<Like>().GetAll()
                    .Any(like => like.UserId == currentUserId && like.ArticleId == articleId),
                    TagId = _uow.GetRepository<ArticleTag>().GetAll()
                    .Include(a => a.Tags)
                    .Where(b => b.ArticleId == articleId)
                    .Select(d => new TagDto()
                    {
                        TagId = d.Tags.Id,
                        TagName = d.Tags.TagName
                    })
                });
        }

        //Get your feed articles//Get your feed articles
        public ShowArticleDto GetArticleByListUserId(int currentPage, int pageRange)
        {
            var currentUserId = _userHelper.GetUserId();

            double maxPage = GetMaxPage();

            var followerIdList = _uow.GetRepository<UserFollow>().GetAll()
                .Include(c => c.Follow)
                .Where(c => c.UserId == currentUserId)
                .Select(c => new FollowDto()
                {
                    followerId = c.Follow.FollowerId
                });

            List<Guid> listFollowerIds = new List<Guid>();

            foreach (var userFollow in followerIdList)
            {
                listFollowerIds.Add(userFollow.followerId);
            }

            List<ArticleGlobalFeedDto> ArticleList = _uow.GetRepository<Article>().GetAll()
                .Include(c => c.User)
                .Include(c => c.ArticleTags)
                .Where(a => listFollowerIds.Contains(a.CreatedByUid))
                .OrderByDescending(c => c.DateTime)
                .Skip((currentPage - 1) * pageRange).Take(pageRange)
                .Select(c => new ArticleGlobalFeedDto()
                {
                    ArticleId = c.Id,
                    Id = c.User.Id,
                    Username = c.User.UserName,
                    Img = c.User.Img,
                    DateTime = c.DateTime,
                    Title = c.Title,
                    Description = c.Description,
                    Like = c.Like,
                    TagId = _uow.GetRepository<ArticleTag>().GetAll()
                    .Include(a => a.Tags)
                    .Where(b => b.ArticleId == c.Id)
                    .Select(d => new TagDto()
                    {
                        TagId = d.Tags.Id,
                        TagName = d.Tags.TagName
                    }),
                    isLike = false
                }).ToList();

            List<bool> isLikeList = new List<bool>();

            foreach (var article in ArticleList)
            {
                isLikeList.Add(_uow.GetRepository<Like>().GetAll()
                    .Any(like => like.UserId == currentUserId && like.ArticleId == article.ArticleId));
            }

            List<ArticleGlobalFeedDto> globalFeedDtosList = new List<ArticleGlobalFeedDto>();

            for (int i = 0; i < ArticleList.Count(); i++)
            {
                globalFeedDtosList.Add(new ArticleGlobalFeedDto()
                {
                    ArticleId = ArticleList.ElementAt(i).ArticleId,
                    Id = ArticleList.ElementAt(i).Id,
                    Username = ArticleList.ElementAt(i).Username,
                    Img = ArticleList.ElementAt(i).Img,
                    DateTime = ArticleList.ElementAt(i).DateTime,
                    Title = ArticleList.ElementAt(i).Title,
                    Description = ArticleList.ElementAt(i).Description,
                    Like = ArticleList.ElementAt(i).Like,
                    TagId = ArticleList.ElementAt(i).TagId,
                    isLike = isLikeList.ElementAt(i)
                });
            }

            return new ShowArticleDto()
            {
                ArticleGlobalFeedDto = globalFeedDtosList,
                MaxPage = maxPage
            };
        }


        //Get Your Articles
        public ShowArticleDto GetArticleByCurrentUserId(int currentPage, int pageRange)
        {
            var currentUserId = _userHelper.GetUserId();
            double maxPage = GetMaxPage();

            List<ArticleGlobalFeedDto> ArticleList = _uow.GetRepository<Article>().GetAll()
                .Include(c => c.User)
                .Include(c => c.ArticleTags)
                .Where(a => a.CreatedByUid == currentUserId)
                .OrderByDescending(c => c.DateTime)
                .Skip((currentPage - 1) * pageRange).Take(pageRange)
                .Select(c => new ArticleGlobalFeedDto()
                {
                    ArticleId = c.Id,
                    Id = c.User.Id,
                    Username = c.User.UserName,
                    Img = c.User.Img,
                    DateTime = c.DateTime,
                    Title = c.Title,
                    Description = c.Description,
                    Like = c.Like,
                    TagId = _uow.GetRepository<ArticleTag>().GetAll()
                    .Include(a => a.Tags)
                    .Where(b => b.ArticleId == c.Id)
                    .Select(d => new TagDto()
                    {
                        TagId = d.Tags.Id,
                        TagName = d.Tags.TagName
                    }),
                    isLike = false
                }).ToList();


            List<bool> isLikeList = new List<bool>();

            foreach (var article in ArticleList)
            {
                isLikeList.Add(_uow.GetRepository<Like>().GetAll()
                    .Any(like => like.UserId == currentUserId && like.ArticleId == article.ArticleId));
            }

            List<ArticleGlobalFeedDto> globalFeedDtosList = new List<ArticleGlobalFeedDto>();

            for (int i = 0; i < ArticleList.Count(); i++)
            {
                globalFeedDtosList.Add(new ArticleGlobalFeedDto()
                {
                    ArticleId = ArticleList.ElementAt(i).ArticleId,
                    Id = ArticleList.ElementAt(i).Id,
                    Username = ArticleList.ElementAt(i).Username,
                    Img = ArticleList.ElementAt(i).Img,
                    DateTime = ArticleList.ElementAt(i).DateTime,
                    Title = ArticleList.ElementAt(i).Title,
                    Description = ArticleList.ElementAt(i).Description,
                    Like = ArticleList.ElementAt(i).Like,
                    TagId = ArticleList.ElementAt(i).TagId,
                    isLike = isLikeList.ElementAt(i)
                });
            }

            return new ShowArticleDto()
            {
                ArticleGlobalFeedDto = globalFeedDtosList,
                MaxPage = maxPage
            };
        }

        public ShowArticleDto GetArticleByUserName(string userName, int currentPage, int pageRange)
        {
            double maxPage = GetMaxPage();

            List<ArticleGlobalFeedDto> ArticleList = _uow.GetRepository<Article>().GetAll()
                .Include(c => c.User)
                .Include(c => c.ArticleTags)
                .Where(a => a.User.UserName == userName)
                .OrderByDescending(c => c.DateTime)
                .Skip((currentPage - 1) * pageRange).Take(pageRange)
                .Select(c => new ArticleGlobalFeedDto()
                {
                    ArticleId = c.Id,
                    Id = c.User.Id,
                    Username = c.User.UserName,
                    Img = c.User.Img,
                    DateTime = c.DateTime,
                    Title = c.Title,
                    Description = c.Description,
                    Like = c.Like,
                    TagId = _uow.GetRepository<ArticleTag>().GetAll()
                    .Include(a => a.Tags)
                    .Where(b => b.ArticleId == c.Id)
                    .Select(d => new TagDto()
                    {
                        TagId = d.Tags.Id,
                        TagName = d.Tags.TagName
                    }),
                    isLike = false
                }).ToList();


            List<bool> isLikeList = new List<bool>();

            foreach (var article in ArticleList)
            {
                isLikeList.Add(_uow.GetRepository<Like>().GetAll()
                    .Include(c => c.Users)
                    .Any(like => like.Users.UserName == userName && like.ArticleId == article.ArticleId));
            }

            List<ArticleGlobalFeedDto> globalFeedDtosList = new List<ArticleGlobalFeedDto>();

            for (int i = 0; i < ArticleList.Count(); i++)
            {
                globalFeedDtosList.Add(new ArticleGlobalFeedDto()
                {
                    ArticleId = ArticleList.ElementAt(i).ArticleId,
                    Id = ArticleList.ElementAt(i).Id,
                    Username = ArticleList.ElementAt(i).Username,
                    Img = ArticleList.ElementAt(i).Img,
                    DateTime = ArticleList.ElementAt(i).DateTime,
                    Title = ArticleList.ElementAt(i).Title,
                    Description = ArticleList.ElementAt(i).Description,
                    Like = ArticleList.ElementAt(i).Like,
                    TagId = ArticleList.ElementAt(i).TagId,
                    isLike = isLikeList.ElementAt(i)
                });
            }

            return new ShowArticleDto()
            {
                ArticleGlobalFeedDto = globalFeedDtosList,
                MaxPage = maxPage
            };
        }

        //Get Favourite Articles (Articles you have liked)
        public ShowArticleDto GetArticlebyLikeArticleId(string userName, int currentPage, int pageRange)
        {
            var currentUserId = _userHelper.GetUserId();
            List<Guid> listArticleId = new List<Guid>();

            double maxPage = GetMaxPage();

            var result = _uow.GetRepository<Like>().GetAll()
                .Include(c => c.Users)
                .Where(like => like.Users.UserName == userName);

            foreach (var x in result)
            {
                listArticleId.Add(x.ArticleId);
            }


            List<ArticleGlobalFeedDto> ArticleList = _uow.GetRepository<Article>().GetAll()
                .Include(c => c.User)
                .Include(c => c.ArticleTags)
                .Where(a => listArticleId.Contains(a.Id))
                .OrderByDescending(c => c.DateTime)
                .Skip((currentPage - 1) * pageRange).Take(pageRange)
                .Select(c => new ArticleGlobalFeedDto()
                {
                    ArticleId = c.Id,
                    Id = c.User.Id,
                    Username = c.User.UserName,
                    Img = c.User.Img,
                    DateTime = c.DateTime,
                    Title = c.Title,
                    Description = c.Description,
                    Like = c.Like,
                    TagId = _uow.GetRepository<ArticleTag>().GetAll()
                    .Include(a => a.Tags)
                    .Where(b => b.ArticleId == c.Id)
                    .Select(d => new TagDto()
                    {
                        TagId = d.Tags.Id,
                        TagName = d.Tags.TagName
                    }),
                    isLike = false
                }).ToList();

            List<bool> isLikeList = new List<bool>();

            foreach (var article in ArticleList)
            {
                isLikeList.Add(_uow.GetRepository<Like>().GetAll()
                    .Any(like => like.UserId == currentUserId && like.ArticleId == article.ArticleId));
            }

            List<ArticleGlobalFeedDto> globalFeedDtosList = new List<ArticleGlobalFeedDto>();

            for (int i = 0; i < ArticleList.Count(); i++)
            {
                globalFeedDtosList.Add(new ArticleGlobalFeedDto()
                {
                    ArticleId = ArticleList.ElementAt(i).ArticleId,
                    Id = ArticleList.ElementAt(i).Id,
                    Username = ArticleList.ElementAt(i).Username,
                    Img = ArticleList.ElementAt(i).Img,
                    DateTime = ArticleList.ElementAt(i).DateTime,
                    Title = ArticleList.ElementAt(i).Title,
                    Description = ArticleList.ElementAt(i).Description,
                    Like = ArticleList.ElementAt(i).Like,
                    TagId = ArticleList.ElementAt(i).TagId,
                    isLike = isLikeList.ElementAt(i)
                });
            }

            return new ShowArticleDto()
            {
                ArticleGlobalFeedDto = globalFeedDtosList,
                MaxPage = maxPage
            };
        }



        //Create new Article
        public Guid AddNewArticle(ArticleCreateNewDto articleCreateNewDto)
        {
            var currentUserId = _userHelper.GetUserId();

            List<Guid> listTagId = new List<Guid>();

            foreach (var tagName in articleCreateNewDto.TagName)
            {
                var result = _uow.GetRepository<Tag>().GetAll().FirstOrDefault(c => c.TagName.Equals(tagName));
                if (result != null)
                {
                    result.TagCount++;
                    listTagId.Add(result.Id);
                }
                else
                {
                    var tag = new Tag()
                    {
                        Id = new Guid(),
                        TagName = tagName,
                        TagCount = 1
                    };

                    _uow.GetRepository<Tag>().Insert(tag);
                    listTagId.Add(tag.Id);
                }

            }

            //var listTagId = _tagDao.AddTag(articleCreateNewDto.TagName);

            var article = new Article()
            {
                Id = new Guid(),
                CreatedByUid = currentUserId,
                Title = articleCreateNewDto.Title,
                Description = articleCreateNewDto.Description,
                Content = articleCreateNewDto.Content,
                DateTime = DateTime.Now
            };

            _uow.GetRepository<Article>().Insert(article);

            foreach (var tagId in listTagId)
            {
                var articleTag = new ArticleTag()
                {
                    ArticleId = article.Id,
                    TagId = tagId
                };
                _uow.GetRepository<ArticleTag>().Insert(articleTag);
            }
            _uow.SaveChange();

            return article.Id;
        }


        //Edit your Article
        public void EditArticle(ArticleEditDto articleEditDto, Guid articleId)
        {
            var result = _uow.GetRepository<ArticleTag>().GetAll().Where(c => c.ArticleId == articleId);

            foreach (var articleTag in result)
            {
                _uow.GetRepository<ArticleTag>().Delete(articleTag);

                var tag = _uow.GetRepository<Tag>().GetAll().Where(c => c.Id == articleTag.TagId);
                foreach (var tagTemp in tag)
                {
                    --tagTemp.TagCount;
                }
            }

            List<Guid> listTagId = new List<Guid>();
            foreach (var tagName in articleEditDto.TagName)
            {
                var tags = _uow.GetRepository<Tag>().GetAll().FirstOrDefault(c => c.TagName.Equals(tagName));
                if (tags != null)
                {
                    tags.TagCount = tags.TagCount++;
                    listTagId.Add(tags.Id);
                }
                else
                {
                    var tag = new Tag()
                    {
                        Id = new Guid(),
                        TagName = tagName,
                        TagCount = 1
                    };

                    _uow.GetRepository<Tag>().Insert(tag);
                    listTagId.Add(tag.Id);

                }
            }

            var article = _uow.GetRepository<Article>().GetAll().First(a => a.Id == articleId);
            if (article != null)
            {
                article.Title = articleEditDto.Title;
                article.Description = articleEditDto.Description;
                article.Content = articleEditDto.Content;
                _uow.GetRepository<Article>().Update(article);
            }

            foreach (var tagId in listTagId)
            {
                var articleTag = new ArticleTag()
                {
                    ArticleId = articleId,
                    TagId = tagId
                };
                _uow.GetRepository<ArticleTag>().Insert(articleTag);
            }
            _uow.SaveChange();
        }

        //Delete your article
        public bool DeleteArticle(Guid articleId)
        {
            var currentUserId = _userHelper.GetUserId();

            var article = _uow.GetRepository<Article>().GetAll().FirstOrDefault(c => c.CreatedByUid == currentUserId && c.Id == articleId);

            if (article != null)
            {
                _uow.GetRepository<Article>().Delete(article);
                _uow.SaveChange();
                return true;
            }

            return false;
        }
    }
}