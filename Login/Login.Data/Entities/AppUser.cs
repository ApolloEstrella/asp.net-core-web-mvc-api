using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Data.Entities
{
    public class AppUser : Microsoft.AspNetCore.Identity.IdentityUser
    {
        [MaxLength(50)]
         public string LastName { get; set; }

    }
}
