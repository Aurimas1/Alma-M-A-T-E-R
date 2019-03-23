using Microsoft.AspNetCore.Identity;

namespace API
{
    public class User : IdentityUser<string>
    {
        public string UserId
        {
            get;
            set;
        }
    }
}
