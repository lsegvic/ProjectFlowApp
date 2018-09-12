using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PracenjeTijekaProjekta.Models;
using PracenjeTijekaProjekta.Models.PracenjeProjekta;
using PagedList;

namespace PracenjeTijekaProjekta.Controllers
{
    public class UsersProjectsController : Controller
    {
        private ProjectContext db = new ProjectContext();
        ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, bool chboxPM = false)
        {
            if (!User.IsInRole("Admin") && !User.IsInRole("Student") && !User.IsInRole("Project manager"))
            {
                return View();
            }
            else
            {
                if (page == null)
                {
                    page = 1;
                }
                ViewBag.CurrentSort = sortOrder;
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

                if (searchString != null)
                {
                    page = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                var projects = from x in db.UsersProjects select x;

                if (User.IsInRole("Admin"))
                {
                    if (chboxPM == true)
                        projects = projects.Where(x => x.ProjectManager == null);
                    ViewBag.isChecked = chboxPM;
                }
                else if (User.IsInRole("Project manager"))
                {
                    projects = projects.Where(x => x.ProjectManager == User.Identity.Name);
                    ApplicationUser user = context.Users.Where(u => u.UserName.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                    ViewBag.UsersNameSurname = user.Name + " " + user.Surname + " Board";
                }
                else if (User.IsInRole("Student"))
                {
                    projects = projects.Where(x => x.UserId == User.Identity.Name);
                }
                if (!String.IsNullOrEmpty(searchString))
                {
                    projects = projects.Where(s => s.ProjectName.Contains(searchString));

                }
                switch (sortOrder)
                {
                    case "Date":
                        projects = projects.OrderBy(s => s.Date);
                        break;
                    case "date_desc":
                        projects = projects.OrderByDescending(s => s.Date);
                        break;
                    default:  // Name ascending 
                        projects = projects.OrderBy(s => s.ProjectName);
                        break;
                }

                int pageSize = 6;
                int pageNumber = (page ?? 1);
                return View(projects.ToPagedList(pageNumber, pageSize));
            }
        }
       
        // GET: UsersProjects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsersProjects/Create     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProjectId,ProjectName")] UsersProject usersProject)
        {
            usersProject.UserId = User.Identity.Name;
            usersProject.Date = DateTime.Now;
            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(User.Identity.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            usersProject.UsersNameSurname = user.Name + " " + user.Surname;
            
            if (ModelState.IsValid)
            {                         
                db.UsersProjects.Add(usersProject);
                db.SaveChanges();           
                return RedirectToAction("Index");
            }
            
            return View(usersProject);
        }

        // GET: UsersProjects/Delete/
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsersProject usersProject = db.UsersProjects.Find(id);
            if (usersProject == null)
            {
                return HttpNotFound();
            }
            return View(usersProject);
        }
        // POST: UsersProjects/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UsersProject usersProject = db.UsersProjects.Find(id);
            db.UsersProjects.Remove(usersProject);
            List<Project> projectDetailsList = db.Projects.Where(u => u.ProjectName == usersProject.ProjectName).ToList();
            foreach (Project project in projectDetailsList)
            db.Projects.Remove(project);
            db.SaveChanges();           
            
            return RedirectToAction("Index");
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsersProject usrproject = db.UsersProjects.Find(id);
            if (usrproject == null)
            {
                return HttpNotFound();
            }
            return View(usrproject);
        }

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
