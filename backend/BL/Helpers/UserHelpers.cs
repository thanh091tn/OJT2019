using Microsoft.AspNetCore.Mvc;
namespace BL.Helpers
{
    public class UserHelpers : ControllerBase
    {
        public string GetUserId()
        {
            var a = HttpContext.Request.Headers["Authorization"];
            return a;
        }
    }
}
