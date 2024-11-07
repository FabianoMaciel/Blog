using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
