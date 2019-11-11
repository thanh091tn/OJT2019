using System.Collections.Generic;

namespace BO.Models
{
    public class User : BaseModel
    {

        public string Password { get; set; }
        public string UserName { get; set; }
        public string About { get; set; }
        public string Email { get; set; }
        public string Img { get; set; }
        /*public  UserFollow UserFollow { get; set; }*/

        public ICollection<UserFollow> UserFollows { get; set; }
        public ICollection<Like> Likes { get; set; }

    }
}
