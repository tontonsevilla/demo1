using AspDotNetCoreDemo.Infrastrcuture.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspDotNetCoreDemo.Domain.Interfaces
{
    public interface IBlogService
    {
        Task<IEnumerable<Blog>> GetBlogs();
    }
}
