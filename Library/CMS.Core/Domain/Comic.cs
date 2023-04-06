using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Core.Domain
{
    public class Comic
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}