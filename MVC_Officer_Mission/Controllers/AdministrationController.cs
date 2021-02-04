using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using MVC_Officer_Mission.Models;
using MVC_Officer_Mission.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVC_Officer_Mission.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private  ApplicationUserManager _userManager;
        private ApplicationDbContext _db = new ApplicationDbContext();

        public AdministrationController()
        {

        }

        public AdministrationController(RoleManager<IdentityRole> roleManager, ApplicationUserManager userManager)
            {
                _roleManager = roleManager;
                UserManager = userManager;
            }

        public ActionResult ListOfRoles()
        {
            var roles = _db.Roles;
            return View(roles);
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        [HttpGet]
        public async Task<ActionResult> EditUserInRole(string RoleId)
        {
            ViewBag.RoleId = RoleId;
            var role = _db.Roles.Find(RoleId);
            if (role == null)
            {
                return View("NotFound");
            }
            ViewBag.RoleName = role.Name;

            var ListUserRole = new List<UserRoleViewModel>();
            foreach (var user in _db.Users)
            {
                var UserRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await UserManager.IsInRoleAsync(user.Id ,  role.Name))
                {
                    UserRoleViewModel.IsSelected = true;
                }
                else
                {
                    UserRoleViewModel.IsSelected = false;
                }
                UserRoleViewModel.RoleId = RoleId;
                ListUserRole.Add(UserRoleViewModel);
            }
            return View(ListUserRole);
        }
        [HttpPost]
        public async Task<ActionResult> EditUserInRole(List<UserRoleViewModel> ListUserRole, string RoleId)
        {
            var role =  _db.Roles.Find(RoleId);
            if (role == null)
            {
                return View("NotFound");
            }
            for (int i = 0; i < ListUserRole.Count; i++)
            {
                var user = await UserManager.FindByIdAsync(ListUserRole[i].UserId);
                IdentityResult result = null;
                if (ListUserRole[i].IsSelected && !(await UserManager.IsInRoleAsync(user.Id, role.Name)))
                {
                    result = await UserManager.AddToRoleAsync(user.Id, role.Name);
                }
                else
                if (!ListUserRole[i].IsSelected && await UserManager.IsInRoleAsync(user.Id, role.Name))
                {
                    result = await UserManager.RemoveFromRoleAsync(user.Id, role.Name);
                }
                else
                {
                    continue;
                }


            }
            return View("ListOfRoles", _db.Roles);
        }
    }

}