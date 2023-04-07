using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Core.Domain
{

    public class Comic
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Thumb { get; set; }
        public string Url { get; set; }
        public ICollection<Series> Series { get; set; } = new List<Series>();
    }
}