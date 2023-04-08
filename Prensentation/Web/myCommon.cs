using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Core.Domain;
using CMS.Data.EFCore;

namespace Web
{
    public static class myCommon
    {

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
        public static void SeedingData()
        {
            using (var _context = new CrawlDB())
            {
                Comic comic = new Comic()
                {
                    Title = "Truyện Tây Du Ký",
                    Thumb = "https://i.ibb.co/QJrR6LD/IMG-5907.jpg",
                    Series = new List<Series>(){
                        new Series{ Content ="<h2>Content 1<h2>" , Chap="1"},
                        new Series {Content ="<h2>Content 2<h2>",  Chap="2"  }
                    }
                };

                Comic comic2 = new Comic()
                {
                    Title = "Anh Hùng Xạ Điêu",
                    Thumb = "https://i.ibb.co/gSMDM97/IMG-5894.jpg",
                    Series = new List<Series>(){
                        new Series{ Content ="<h2>Content 1<h2>" , Chap="1"},
                        new Series {Content = "<h2>Content 2<h2>", Chap="2"  },
                        new Series {Content = "<h2>Content 3<h2>", Chap="3"  }
                    }
                };

                comic.Url = myCommon.FriendlyUrl(comic.Title);
                comic2.Url = myCommon.FriendlyUrl(comic2.Title);

                _context.Comics.Add(comic);
                _context.Comics.Add(comic2);
                _context.SaveChanges();
            }
        }

        public static string GetUrlTitle(string url)
        {
            var split = url.Split("/");
            return split[split.Length - 1];
        }
    }
}