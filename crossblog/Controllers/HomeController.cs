using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using crossblog.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace crossblog.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {

        [HttpGet("[action]")]
        public IActionResult Index()
        {
            return Content("Crossover Blog Api", "text/plan");
        }

        [HttpGet("[action]/{code}")]
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code));
        }

    }
}