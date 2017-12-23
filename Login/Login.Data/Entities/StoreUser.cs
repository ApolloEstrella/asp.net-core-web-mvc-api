 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Data.Entities
{
    public class StoreUser : Microsoft.AspNetCore.Identity.IdentityUser
    {
        [MaxLength(50)]
        string LastName { get; set; }
        [MaxLength(50)]
        string FirstName { get; set; }
        [MaxLength(50)]
        string Middle { get; set; }
    }
}
