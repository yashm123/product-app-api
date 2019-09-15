using System;
using System.Collections.Generic;

namespace ProductApi.Models
{
    public partial class UserInfo
    {
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Gender { get; set; }
        public long? MobileNumber { get; set; }
    }
}
