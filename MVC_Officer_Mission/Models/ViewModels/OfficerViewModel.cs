using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Officer_Mission.Models.ViewModels
{
    public class OfficerExternalTournament
    {
        public string Name { get; set; }
    }
    public class OfficerViewModel
    {
        public OfficerViewModel()
        {
            this.OfficerExternalTournaments = new List<OfficerExternalTournament>();
        }
        public Officer Officer { get; set; }
        public List<OfficerExternalTournament> OfficerExternalTournaments { get; set; }
    }
}