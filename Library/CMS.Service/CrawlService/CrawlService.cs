using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CMS.Core.Domain;
using CMS.Data.EFCore;
using HtmlAgilityPack;

namespace CMS.Service.CrawlService
{
    public class CrawlService : ICrawlService
    {

        public int CheckPaging(HtmlDocument htmlDoc)
        {
            int page = 0;
            Console.WriteLine("Chek paging is processing!!!");
            //Check if paging has Cuối
            var nodesPaging = htmlDoc.DocumentNode.SelectNodes("//ul[@class='pagination pagination-sm']//li//a");
            Console.WriteLine("Has Nodes ? Yess");

            foreach (var item in nodesPaging)
            {
                if (item.InnerText == "Cuối &raquo;")
                {
                    Console.WriteLine("Co trang cuoi ? ");

                    if (item.Attributes["title"] != null)
                    {
                        if (item.Attributes["title"].Value != null)
                        {
                            string pageAsString = item.Attributes["title"].Value;
                            // Console.WriteLine($"Title {pageAsString}");
                            // Console.WriteLine(pageAsString.Substring(pageAsString.Length-1, 1));
                            // string pageTest = pageAsString.Substring(pageAsString[pageAsString.Length - 1], 1);
                            // Console.WriteLine(pageTest);
                            page = int.Parse(pageAsString.Substring(pageAsString.Length - 1, 1));
                            Console.WriteLine($"Page is {page}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Trang cuoi la 4");

                    page = 4;
                }

            }
            return page;
        }

        public async Task CrawlComicAsync(string url)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            string htmlString = await GetHtmlStringAsync(url);
            if (htmlString != null && htmlString != "")
            {
                htmlDoc.LoadHtml(htmlString);
            }

            //Check if Comic exist
            string urlComic = GetUrlTitle(url);
            Console.WriteLine(urlComic);
            using (var context = new CrawlDB())
            {
                var comic = context.Comics.FirstOrDefault(s => s.Url == urlComic);
                Console.WriteLine($"has comic yet? {comic} ");
                if (comic is null)
                {
                    MetaModel meta = GetMetaComic(htmlDoc);
                    comic = new Comic
                    {
                        Title = meta.Title,
                        Thumb = meta.Image,
                        Url = FriendlyUrl(meta.Title)
                    };
                    Console.WriteLine($"Add comic model {comic} ");

                    context.Comics.Add(comic);
                    context.SaveChanges();
                }
            }

            int page = CheckPaging(htmlDoc);
            Console.WriteLine($"Check page is {page} ");


            for (int i = 0; i < page; i++)
            {
                if (i == 0)
                {
                    List<string> lstLinkCrawling = new List<string>();
                    var nodeList = htmlDoc.DocumentNode.SelectNodes("//ul[@class='list-chapter']//li//a");
                    foreach (var node in nodeList)
                    {
                        if (node.Attributes["href"] != null)
                        {
                            if (node.Attributes["href"].Value != null)
                            {
                                lstLinkCrawling.Add(WebUtility.HtmlDecode(node.Attributes["href"].Value.ToString()));
                            }
                        }

                    }

                    int j = i * 10;
                    foreach (string urlContent in lstLinkCrawling)
                    {
                        string content = await GetContentChapterAsync(urlContent);
                        Series series = new Series
                        {
                            Content = content,
                            Chap = j++.ToString(),
                            Url = urlComic + "/" + "trang-" + (i + 1).ToString(),
                        };

                        using (var context = new CrawlDB())
                        {
                            var comic = context.Comics.FirstOrDefault(s => s.Url == urlComic);
                            if (comic != null)
                            {
                                comic.Series.Add(series);
                            }
                            context.SaveChanges();
                        }
                    }
                }
            }
        }

        public async Task<string> GetContentChapterAsync(string url)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            string htmlString = await GetHtmlStringAsync(url);
            if (htmlString != null && htmlString != "")
            {
                htmlDoc.LoadHtml(htmlString);
            }
            HtmlNode nodeUrlLink = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='chapter-c']");
            var content = nodeUrlLink.InnerHtml;
            return content;
        }

        public async Task<string> GetHtmlStringAsync(string url)
        {
            using (var request = new HttpClient())
            {
                request.DefaultRequestHeaders.Add("User-Agent", "C# program");
                return await request.GetStringAsync(url);
            }
        }

        public MetaModel GetMetaComic(HtmlDocument htmlDoc)
        {
            MetaModel meta = new MetaModel();
            HtmlNodeCollection metaNodes = SelectMetaNodes("meta", "property", "og:", htmlDoc);

            if (metaNodes != null)
            {
                foreach (HtmlNode node in metaNodes)
                {
                    if (node.Attributes["property"].Value.ToLower() == "og:title")
                    {
                        //Giải mã các kí tự " & < >... có trong title
                        meta.Title = WebUtility.HtmlDecode(node.Attributes["content"].Value);
                    }
                    if (node.Attributes["property"].Value.ToLower() == "og:description")
                    {
                        meta.Description = WebUtility.HtmlDecode(node.Attributes["content"].Value);
                    }
                    if (node.Attributes["property"].Value.ToLower() == "og:image")
                    {
                        meta.Image = node.Attributes["content"].Value;
                    }
                }
            }
            return meta;
        }

        public void ProcessUrl(string url)
        {
            throw new NotImplementedException();
        }

        public void WriteLog()
        {
            throw new NotImplementedException();
        }

        public HtmlNodeCollection SelectMetaNodes(string element, string attribute, string name, HtmlDocument HtmlDocument)
        {
            return HtmlDocument.DocumentNode.SelectNodes($"//{element}[contains(@{attribute},'{name}')]");
        }

        public static string GetUrlTitle(string url)
        {
            var split = url.Split("/");
            return split[split.Length - 2];
        }



        public static string FriendlyUrl(string strTitle)
        {
            return ReplaceSpecial(strTitle);
        }

        public static string ReplaceSpecial(string title)
        {
            if (title == null) return string.Empty;

            const int maxlen = 500;
            int len = title.Length;
            bool prevdash = false;
            var sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = title[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    sb.Append((char)(c | 32));
                    prevdash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
                    c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevdash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
                else if ((int)c >= 128)
                {
                    int prevlen = sb.Length;
                    sb.Append(RemapInternationalCharToAscii(c));
                    if (prevlen != sb.Length) prevdash = false;
                }
                if (i == maxlen) break;
            }

            if (prevdash)
                return sb.ToString().Substring(0, sb.Length - 1);
            else
                return sb.ToString();
        }

        public static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("áàảãạăắằẳẵặâấầẩẫậ".Contains(s))
            {
                return "a";
            }
            else if ("éèẻẽẹêếềểễệ".Contains(s))
            {
                return "e";
            }
            else if ("íìỉĩị".Contains(s))
            {
                return "i";
            }
            else if ("óòỏõọôốồổỗộơớờởỡợ".Contains(s))
            {
                return "o";
            }
            else if ("úùủũụưứừửữự".Contains(s))
            {
                return "u";
            }
            else if ("ýỳỷỹỵ".Contains(s))
            {
                return "y";
            }
            else if ("đ".Contains(s))
            {
                return "d";
            }
            else
            {
                return "";
            }
        }

        public static string GetChapterViaUrl(string url)
        {
            var split = url.Split("/");
            var urlTitle = split[split.Length - 2];
            return urlTitle.Substring(urlTitle.Length - 1, 1);
        }
    }

}
