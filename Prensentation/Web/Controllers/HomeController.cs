using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CMS.Core.Domain;
using CMS.Data.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        // private readonly IComicService _service;
        // public HomeController(IComicService service)
        // {
        //     _service = service;
        // }

        public IActionResult Index()
        {
            List<Comic> comics = new List<Comic>();
            using (var context = new CrawlDB())
            {
                comics = context.Comics.Include(x => x.Series).ToList();
            }
            return View(comics);
        }

        [Route("{url}")]
        public IActionResult DetailComic(string url)
        {
            Comic comic = new Comic();
            using (var context = new CrawlDB())
            {
                comic = context.Comics.Include(x => x.Series).FirstOrDefault(x => x.Url == url);
            }
            return View(comic);
        }

        [Route("{url}/chuong-{chap}")]
        public IActionResult Read(string url, string chap)
        {
            Comic comic = new Comic();
            using (var context = new CrawlDB())
            {
                comic = context.Comics.Include(x => x.Series).FirstOrDefault(x => x.Url == url);
            }

            ComicViewModel comicViewModel = new ComicViewModel
            {

                Title = comic.Title,
                Chap = comic.Series.Where(x => x.Chap == chap).FirstOrDefault().Chap,
                Content = comic.Series.Where(x => x.Chap == chap).FirstOrDefault().Content,
                Url = comic.Url
            };

            return View(comicViewModel);
        }

        public IActionResult Privacy()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
