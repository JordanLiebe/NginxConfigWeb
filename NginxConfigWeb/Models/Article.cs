using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NginxConfigWeb.Models
{
    public class Article
    {
        [Display(Name = "Name")]
        public string name { get; set; }
        [Display(Name = "Content")]
        public string content { get; set; }
        [Display(Name = "Creator")]
        public string creator { get; set; }
        [Display(Name = "Created")]
        public DateTime created { get; set; }
    }
}
