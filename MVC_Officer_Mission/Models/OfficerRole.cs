using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MVC_Officer_Mission.Models
{
    public class OfficerRole
    {
        public int Id { get; set; }

        [DisplayName("وظيفة الضابط")]
        public string Name { get; set; }
    }
}