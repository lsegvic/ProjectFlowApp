using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PracenjeTijekaProjekta.Models;
using PracenjeTijekaProjekta.Models.PracenjeProjekta;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity.Infrastructure;

namespace PracenjeTijekaProjekta.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        ApplicationDbContext context = new ApplicationDbContext();
        private ProjectContext db = new ProjectContext();

        //GET
        public ActionResult AssignRole()
        {
            //prikaz svih usera koji nemaju pridjeljenu rolu
            string query = "select * from dbo.AspNetUsers where Id not in(select UserId from dbo.AspNetUserRoles)";
            IEnumerable<IdentityUser> usersWithoutRole = db.Database.SqlQuery<IdentityUser>(query);
            ViewBag.Users = new SelectList(usersWithoutRole.Select(x => x.UserName));

            //prikaz svih rola u dropdown listi
            List<SelectListItem> roles = context.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();
            ViewBag.Roles = roles;

            return View();
        }
        [HttpPost]
        public ActionResult AssignRole(FormCollection from)
        {
            string query = "select * from dbo.AspNetUsers " +
                           "where Id not in" +
                           "(select UserId from dbo.AspNetUserRoles)";

            IEnumerable<IdentityUser> usersWithoutRole = db.Database.SqlQuery<IdentityUser>(query);
            ViewBag.Users = new SelectList(usersWithoutRole.Select(x => x.UserName));

            List<SelectListItem> roles = context.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();
            ViewBag.Roles = roles;

            string usrname = from["UserEmail"];
            string rolname = from["RoleName"];

            var FieldsAreEmpty = string.IsNullOrEmpty(usrname) || string.IsNullOrEmpty(rolname);

            if (FieldsAreEmpty)
            {
                ModelState.AddModelError("", "Select user and role you want to assign");
                return View();
            }
            else
            {
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(usrname, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                userManager.AddToRole(user.Id, rolname);
            }
            return RedirectToAction("Index", "UsersProjects");
        }

        public ActionResult AssignProjectManager(string ProjectName)
        {
            ViewBag.projectName = ProjectName;
            IQueryable<string> userList = context.Users.Select(u => u.Id);
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            List<SelectListItem> PMList = new List<SelectListItem>();
            foreach (string u in userList)
            {
                if (userManager.IsInRole(u, "Project manager"))
                    PMList.Add(new SelectListItem { Value = userManager.FindById(u).UserName, Text = userManager.FindById(u).UserName });

            }
            ViewBag.projectMng = PMList;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignProjectManager([Bind(Include = "ProjectName,ProjectManager")]  UsersProject usersProject, FormCollection from)
        {
            IQueryable<string> userList = context.Users.Select(u => u.Id);
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            List<SelectListItem> PMList = new List<SelectListItem>();
            foreach (string u in userList)
            {
                if (userManager.IsInRole(u, "Project manager"))
                    PMList.Add(new SelectListItem { Value = userManager.FindById(u).UserName, Text = userManager.FindById(u).UserName });

            }
            ViewBag.projectMng = PMList;
            string username = from["ProjectMng"];
            if (string.IsNullOrEmpty(username))
            {
                ModelState.AddModelError("", "");
                return View();
            }
            else
            {
                UsersProject usproj = db.UsersProjects.Where(pn => pn.ProjectName.Equals(usersProject.ProjectName)).FirstOrDefault();
                usproj.ProjectManager = username;
                db.Entry(usproj).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "UsersProjects");
        }
    }
}