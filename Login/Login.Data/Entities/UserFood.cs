using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Data.Entities
{
    public class UserFood
    {
        public int UserFoodId { get; set; }
        [MaxLength(500)]
        public string FoodNames { get; set; }
        public ApplicationUser User { get; set; }
    }
}
