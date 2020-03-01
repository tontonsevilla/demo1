using AspDotNetCoreDemo.Domain.Interfaces;
using AspDotNetCoreDemo.Domain.Models;
using AspDotNetCoreDemo.Infrastrcuture.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspDotNetCoreDemo.Controllers.api
{
    [Route("api/v1/[controller]", Name = "defaultApi")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogService blogService;

        public BlogsController(
            IBlogService blogService)
        {
            this.blogService = blogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBlogs()
        {
            var result = new ApiResponse<IEnumerable<Blog>>();

            var data = await blogService.GetBlogs();

            result.AddData(data);

            return Ok(result);
        }
    }
}