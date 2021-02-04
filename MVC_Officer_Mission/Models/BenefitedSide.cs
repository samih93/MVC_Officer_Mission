using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC_Officer_Mission.Models
{
    public class BenefitedSide
    {
        public int Id { get; set; }

        [DisplayName("إسم الجهة المستفيدة")]
        public string Name { get; set; }
    }
}