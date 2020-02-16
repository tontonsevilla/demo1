using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspDotNetCoreDemo.Infrastrcuture.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspDotNetCoreDemo
{
    public class HomeController : Controller
    {
        private readonly AspDotNetCoreDemoDatabaseContext dbContext;

        public HomeController(AspDotNetCoreDemoDatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View(this.dbContext.Blogs.ToList());
        }
    }
}
