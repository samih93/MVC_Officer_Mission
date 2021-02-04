using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Officer_Mission.Models
{
    public class MissionLog
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int MissionId { get; set; }
        public string mission_name { get; set; }
        public DateTime DateModified { get; set; }
        public int Action_Id { get; set; }
    }
}