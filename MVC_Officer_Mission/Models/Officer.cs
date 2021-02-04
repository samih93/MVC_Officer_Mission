using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using MVC_Officer_Mission.Controllers;
using System.ComponentModel.DataAnnotations;

namespace MVC_Officer_Mission.Models
{
    public class Officer
    {
        public Officer()
        {
            this.Missions = new HashSet<Mission>();
            this.ExternalTournaments = new HashSet<ExternalTournament>();
            this.Specifications = new HashSet<Specification>();
        }
        public int Id { get; set; }

        [DisplayName("ضمن وحدة المعهد")]
        public bool IsInInstitute { get; set; }

        [DisplayName("الرتبة")]
        public int RankId { get; set; }
        public virtual Rank Rank { get; set; }

        [DisplayName("الرقم المالي")]
        public int MilitaryNumber { get; set; }

        [DisplayName("اسم الضابط")]
        public string FirstName { get; set; }

        [DisplayName("الشهرة")]
        public string LastName { get; set; }

        [DisplayName("إسم الأب")]
        public string FatherName { get; set; }

        //fulll name is first + last
        [DisplayName("إسم الضابط الكامل")]
        public string FullName
        {
            get { return this.Rank.Name + " " + this.FirstName + " " + this.LastName; }
        }

        [DisplayName("إسم الأم وشهرتها")]
        public string MotherFullName { get; set; }

        [DisplayName("تاريخ الولادة")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime BirthDate { get; set; }

        [DisplayName("رقم الهاتف")]
        public string PhoneNumber { get; set; }

        [DisplayName("مكان السكن")]
        public string Address { get; set; }

        [DisplayName("بريد إلكتروني")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("الشهادات الجامعية")]
        public string CollegeCertificates { get; set; }

        [DisplayName("مركز الخدمة")]
        public int? DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        [DisplayName("رئيس قطعة")]
        public bool IsHeadOfDepartment { get; set; }

        [DisplayName("صورة الملف الشخصي")]
        public string ProfileImage { get; set; }

        [DisplayName("التخصص")]
   //     public int? SpecificationId { get; set; }
        public virtual ICollection<Specification> Specifications { get; set; }

        [DisplayName("الدورات الخارجية")]
        public virtual ICollection<ExternalTournament> ExternalTournaments { get; set; }

        public virtual ICollection<Mission> Missions { get; set; }
        public virtual ICollection<OfficerRoleInMission> OfficerRoleInMissions { get; set; }
    }
}