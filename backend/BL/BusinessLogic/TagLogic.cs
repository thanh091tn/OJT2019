using BL.Interfaces;
using BO.Dtos;
using BO.Models;
using DAL.Repository;
using DAL.Repository.UnitOfWorks;
using System.Collections.Generic;
using System.Linq;

namespace BL
{


    public class TagLogic : ITagLogic
    {

        private readonly IUnitOfWork _uow;

        public TagLogic(IUnitOfWork uow)
        {

            _uow = uow;
        }

        //Get list popular order by number of tag count
        public IEnumerable<TagDto> GetPopularTags()
        {
            return _uow.GetRepository<Tag>().GetAll()
                .OrderByDescending(tag => tag.TagCount)
                .Take(20)
                .Select(c => new TagDto()
                {
                    TagId = c.Id,
                    TagName = c.TagName
                });
        }
    }
}