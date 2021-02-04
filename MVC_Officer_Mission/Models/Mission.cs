using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MVC_Officer_Mission.Models
{
    public class Mission
    {
        public Mission()
        {
            this.Officers = new HashSet<Officer>();
        }
        public int Id { get; set; }

        [DisplayName("اسم المهمة")]
        [Required]
        public string Name { get; set; }

        [DisplayName("من")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Required]
        public DateTime From { get; set; }

        [DisplayName("الى")]
        [DisplayFormat(ApplyFormatInEditMode = true,DataFormatString = "{0:yyyy-MM-dd}")]
        [Required]
        public DateTime To  { get; set; }

        [DisplayName("دورة")]
        public bool Istournament { get; set; }
     //   public int  Officer_id { get; set; }

        public virtual ICollection<Officer> Officers { get; set; }


        [ForeignKey("Tournament")]
        public int? TournamentID { get; set; }
        public virtual Tournament Tournament { get; set; }

    }
}