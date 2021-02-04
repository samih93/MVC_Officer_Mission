using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_Officer_Mission.Models
{
    public class Department
    {
        public int Id { get; set; }
        [DisplayName("القطعة")]
        public string  Name { get; set; }
        public virtual ICollection<Officer> Officers { get; set; }
    }
}