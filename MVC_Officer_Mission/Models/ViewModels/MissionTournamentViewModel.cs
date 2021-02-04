using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Officer_Mission.Models.ViewModels
{
    public class TournamentOfficerRole
    {
        public int OfficerId { get; set; }
        public string OfficerRole { get; set; }
    }
    public class MissionTournamentViewModel
    {
        public MissionTournamentViewModel()
        {
            this.listOfTournamentOfficers = new List<TournamentOfficerRole>();
        }
        public Mission Mission { get; set; }
        public Tournament Tournament { get; set; }
        public Officer Officer { get; set; }

        //used for multiple tournament officers dropdowns in create and edit
        public List<TournamentOfficerRole> listOfTournamentOfficers { get; set; }
    }
}