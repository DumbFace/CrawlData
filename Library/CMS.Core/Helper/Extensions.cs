using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Core.Helper
{
    public static class Extensions
    {
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
            var split = url.Split("-");
            //Xoa ky tu / trong url https://truyenfull.vn/anh-hau-thanh-doi/chuong-52/
            return split[split.Length - 1].Replace("/", "");
        }
    }
}