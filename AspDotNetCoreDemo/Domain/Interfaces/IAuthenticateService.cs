using AspDotNetCoreDemo.Domain.Jwt.Models;
using System.Threading.Tasks;

namespace AspDotNetCoreDemo.Domain.Interfaces
{
    public interface IAuthenticateService
    {
        Task<AuthWithToken> IsAuthenticatedAsync(TokenRequest request);   
    }
}
