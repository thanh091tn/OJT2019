using BO.Dtos;
using System.Collections.Generic;

namespace BL.Interfaces
{
    public interface ITagLogic
    {
        IEnumerable<TagDto> GetPopularTags();
    }
}
