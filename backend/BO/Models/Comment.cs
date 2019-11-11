using System;

namespace BO.Models
{
    public class Comment : BaseModel
    {
        public string Context { get; set; }
        public DateTime Date { get; set; }
        public User Users { get; set; }
        public Guid UserId { get; set; }
    }
}
