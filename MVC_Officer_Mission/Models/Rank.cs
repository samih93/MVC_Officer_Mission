using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MVC_Officer_Mission.Models
{
    public class Rank
    {
        public int Id { get; set; }
        [DisplayName("الرتبة")]

        public string Name { get; set; }

        public virtual ICollection<Officer> Officers { get; set; }
    }
}