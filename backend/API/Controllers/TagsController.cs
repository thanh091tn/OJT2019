using BL.Interfaces;
using BO.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace API.Controllers
{

    public class TagsController : BaseController
    {
        private readonly ITagLogic _tagLogic;

        public TagsController(ITagLogic tagLogic)
        {
            _tagLogic = tagLogic;
        }

        // GET: api/Tags
        [HttpGet]
        [AllowAnonymous]
        public IEnumerable<TagDto> GetPopularTags()
        {
            return _tagLogic.GetPopularTags();
        }
    }
}