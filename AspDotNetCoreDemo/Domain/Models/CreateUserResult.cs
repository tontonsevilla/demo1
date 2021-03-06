﻿using Microsoft.AspNetCore.Identity;

namespace AspDotNetCoreDemo.Domain.Models
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
