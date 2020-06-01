using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NginxConfigWeb.Models
{
    class RtmpApplications
    {
        public string name { get; set; }
        public string live { get; set; }
        public string[] push_urls { get; set; }
        public string record { get; set; }
    }
}
