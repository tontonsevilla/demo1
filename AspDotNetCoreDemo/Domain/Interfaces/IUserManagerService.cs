using AspDotNetCoreDemo.Domain.Models;
using System.Threading.Tasks;

namespace AspDotNetCoreDemo.Domain.Interfaces
{
    public interface IUserManagerService
    {
        Task<CreateUserResult> CreateUserAsync(string userName, string password);
        Task<bool> IsValidUser(string userName, string password);
    }
}
