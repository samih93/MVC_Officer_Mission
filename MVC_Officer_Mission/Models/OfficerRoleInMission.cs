using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MVC_Officer_Mission.Models
{
    public class OfficerRoleInMission
    {
        public OfficerRoleInMission()
        {

        }
        public int Id { get; set; }
        public int OfficerId { get; set; }
        public virtual Officer Officer { get; set; }

        [DisplayName("وظيفة الضابط")]
        public string OfficerRole { get; set; }

        //public int OfficerRoleId { get; set; }
        //public virtual OfficerRole OfficerRole { get; set; }

        public int TournamentId { get; set; }
        public virtual Tournament Tournament{ get; set; }

    }
}