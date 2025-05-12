using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoTorz.Model
{
    public partial class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
       
        [NotMapped] // This property is not mapped to the database, its for the UI so we dont update role accendtally
        public string NewRole { get; set; } = string.Empty;
    }
}
