using System;

namespace BO.Models
{
    public class UserFollow : BaseModel
    {
        public User User { get; set; }

        public Guid UserId { get; set; }

        public Follow Follow { get; set; }
        public Guid FollowId { get; set; }
    }
}
