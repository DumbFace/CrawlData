using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers
{

    public class CrawlController : Controller
    {
        private readonly ILogger<CrawlController> _logger;

        public CrawlController(ILogger<CrawlController> logger)
        {
            _logger = logger;
        }

        public IActionResult CrawlData()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}