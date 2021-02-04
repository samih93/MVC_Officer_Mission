using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MVC_Officer_Mission.Models
{
    public class ExternalTournament
    {
        public int Id{ get; set; }
        [DisplayName("إسم الدورة الخارجية")]
        public string Name { get; set; }
        public int OfficerId { get; set; }
        public virtual Officer Officer { set; get; }
    }
}