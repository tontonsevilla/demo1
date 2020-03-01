using AspDotNetCoreDemo.Domain.Interfaces;
using AspDotNetCoreDemo.Infrastrcuture.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreDemo.Infrastructure.Services
{
    public class BlogService : IBlogService
    {
        private readonly AspDotNetCoreDemoDatabaseContext dbContext;

        public BlogService(
            AspDotNetCoreDemoDatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Blog>> GetBlogs()
        {
            return await Task.Run(() => {
                return dbContext.Blogs.ToList();
            });
        }
    }
}
