using System;
using System.Collections.Generic;

namespace GoTorz.Data
{
    public partial class User
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }
    }
}
