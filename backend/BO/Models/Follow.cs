using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BO.Models
{
    public class Follow : BaseModel
    {
        public User User { get; set; }
        [ForeignKey("User")]
        public Guid FollowerId { get; set; }

        /*public UserFollow UserFollows { get; set; }*/
        public ICollection<UserFollow> UserFollows { get; set; }
    }
}
