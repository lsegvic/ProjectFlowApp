using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PracenjeTijekaProjekta.Models.PracenjeProjekta;

namespace PracenjeTijekaProjekta.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private ProjectContext db = new ProjectContext();

        // GET: Projects
        public ActionResult Index(string ProjectName)
        {
            if (User.IsInRole("Admin") || User.IsInRole("Student") || User.IsInRole("Project manager"))
            {
                int totalDurationOfProject = 0;
                ViewBag.projectName = ProjectName;
                if (ProjectName != null)
                {          
                    List<Project> project = db.Projects.Where(x => x.ProjectName == ProjectName).ToList();
                    foreach (var item in db.Projects.ToList())
                        if (ProjectName == item.ProjectName)
                        {
                            totalDurationOfProject += item.Duration;                           
                        }
                   
                    ViewBag.totalDuration = (totalDurationOfProject/60).ToString("0") + " hours";
                    return View(project);
                }
            }          
            return View();
        }
        
        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // GET: Projects/Create
        public ActionResult Create()
        {
            //iz baze uzima sve kategorije i upisuje u dropdown definiran u view-u create
            Project model = new Project();
            model.CategoryList = new SelectList(db.Categories, "CategoryName", "CategoryName");
            return View(model);
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,Description,Category,Duration,NumP")] Project project, string ProjectName)
        {      
            project.UserId = User.Identity.Name;
            project.ProjectName = ProjectName;         
            ModelState["ProjectName"].Errors.Clear();
           
            try
            {
                if (ModelState.IsValid)
                {
                    db.Projects.Add(project);
                    db.SaveChanges();

                    return RedirectToAction("Index", new { @ProjectName = project.ProjectName });
                }
            }
            catch(DataException e)
            {
                ModelState.AddModelError("", e.Message);

            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
       
            Project project = db.Projects.Find(id);

            if (project == null)
            {
                return HttpNotFound();
            }

            project.CategoryList = new SelectList(db.Categories, "CategoryName", "CategoryName");
            return View(project);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,Description,Category,Duration,NumP")] Project project, string ProjectName)
        {

            project.UserId = User.Identity.Name;
            project.ProjectName = ProjectName;
            ModelState["ProjectName"].Errors.Clear();
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { @ProjectName = project.ProjectName });
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index", new { @ProjectName = project.ProjectName });
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
