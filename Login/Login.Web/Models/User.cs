using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Web.Models
{
    public class User
    {
        public string Email { get; set; }
        [MinLength(6)]
        public string Password { get; set; }
        public string Token { get; set; }
        public string Id { get; set; }
    }
}
