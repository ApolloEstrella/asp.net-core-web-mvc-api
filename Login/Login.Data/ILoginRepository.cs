using System.Collections.Generic;
using Login.Data.Entities;

namespace Login.Data
{
    public interface ILoginRepository
    {
        IEnumerable<ApplicationUser> Getusers();
    }
}