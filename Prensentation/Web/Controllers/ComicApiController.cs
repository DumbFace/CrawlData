using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CMS.Core.Domain;
using CMS.Data.EFCore;
using CMS.Data.Service.ComicService;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/comic")]
    public class ComicApiController : ControllerBase
    {
        private readonly IComicService _service;
        public ComicApiController(IComicService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] Series series)
        {

            // HtmlDocument htmlDoc = new HtmlDocument();
            // string htmlString = await GetHtmlAsync(model.Url);
            // if (htmlString != null && htmlString != "")
            // {
            //     htmlDoc.LoadHtml(htmlString);
            // }
            // HtmlNode nodeUrlLink = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='chapter-c']");
            // var body = nodeUrlLink.InnerHtml;
            // Comic comic= new Comic(){
            //   Content = body  
            // };
            // if (body != null)
            // {
            //     _service.Insert(comic);
            //     _service.Save();
            // }
            using (CrawlDB _context = new CrawlDB())
            {
                Comic comic = new Comic()
                {
                    Title = "Test 2",
                    Series = new List<Series>(){
                        new Series{ Content =" Test 2" },
                        new Series {Content = "TEst 3"}
                    }

                };
                _service.Insert(comic);
                _service.Save();
            }



            return Ok();
        }


        public async Task<string> GetHtmlAsync(string url)
        {
            using (var request = new HttpClient())
            {
                request.DefaultRequestHeaders.Add("User-Agent", "C# program");
                return await request.GetStringAsync(url);
            }
        }
    }
}