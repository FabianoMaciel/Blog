using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Occupation { get; set; }

        public bool IsAdmin { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
