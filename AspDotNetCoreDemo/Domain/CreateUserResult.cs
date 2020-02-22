using Microsoft.AspNetCore.Identity;

namespace AspDotNetCoreDemo.Domain
{
    public class CreateUserResult
    {
        public CreateUserResult()
        {
            RegisterResult = new IdentityResult();
            ConfirmEmailResult = new IdentityResult();
        }

        public IdentityResult RegisterResult { get; set; }
        public IdentityResult ConfirmEmailResult { get; set; }
    }
}
