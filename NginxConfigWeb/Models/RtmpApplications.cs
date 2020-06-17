using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NginxConfigWeb.Models
{
    public class RtmpApplications
    {
        [Display(Name = "Name")]
        public string name { get; set; }
        [Display(Name = "Live")]
        public string live { get; set; }
        [Display(Name = "Push Urls")]
        public string[] push_urls { get; set; }
        [Display(Name = "Record")]
        public string record { get; set; }
    }
}
