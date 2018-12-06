using Microsoft.AspNetCore.Identity;

namespace DutchTreat.Data.Entities
{
        // Own properties definition for our user
    public class StoredUser : IdentityUser
    {
        public string FirtsName { get; set; }
        public string LastName { get; set; }
    }
}