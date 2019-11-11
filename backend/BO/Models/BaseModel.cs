using System;

namespace BO.Models
{
    public class BaseModel
    {
        public Guid Id { get; set; }
        public bool IsDelete { get; set; }
    }
}
