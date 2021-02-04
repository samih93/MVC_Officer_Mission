using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
namespace MVC_Officer_Mission.Models
{
    public class Tournament
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        public Tournament()
        {
            this.OfficersRolesInMission = new HashSet<OfficerRoleInMission>();
        }

        public int Id { get; set; }

        [DisplayName("القطعة")]
        public string Department { get; set; }

        [DisplayName("القاعة")]
        public string Room { get; set; }

        [DisplayName("امر المستند")]
        public string DocumentOrder { get; set; }

        [DisplayName("ملف امر المستند")]
        public string DocumentOrderFile { get; set; }

        [DisplayName("الجهة المستفيدة")]
        public int? BenefitedSideId { get; set; }

        public virtual BenefitedSide BenefitedSide { get; set; }

        [DisplayName("الجهة المانحة")]
        public string Provider { get; set; }


        [DisplayName("تاريخ التخريج")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? GraduationDate { get; set; }

        public virtual ICollection<Mission> Missions { get; set; }
        public virtual ICollection<OfficerRoleInMission> OfficersRolesInMission { get; set; }


    }
}