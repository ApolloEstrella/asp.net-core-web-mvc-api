using Login.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Data
{
    public class LoginRepository : ILoginRepository
    {
        private readonly LoginDbContext _ctx;

        public LoginRepository(LoginDbContext ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<ApplicationUser> Getusers()
        {
            return _ctx.Users.OrderBy(p => p.Email).ToList();
        }

    }
}
