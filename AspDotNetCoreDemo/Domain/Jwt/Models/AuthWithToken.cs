namespace AspDotNetCoreDemo.Domain.Jwt.Models
{
    public class AuthWithToken
    {
        public bool IsAuthenticated { get; set; }
        public string Token { get; set; }
    }
}
