using Microsoft.AspNet.Identity.EntityFramework;
using PracenjeTijekaProjekta.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PracenjeTijekaProjekta.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Roles
        public ActionResult Index()
        {
            return View(db.Roles.ToList());
        }



        // GET: Roles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        [HttpPost]
        public ActionResult Create(IdentityRole role)
        {
            for (int i = 0; i < db.Roles.ToList().Count; i++)
                if (role.Name == db.Roles.ToList()[i].Name)
                {
                    ModelState.AddModelError("", "Role already exists");
                    return View();
                }
            try
            {
                if (ModelState.IsValid)
                {
                    db.Roles.Add(role);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                return View(role);
            }
            catch
            {
                ModelState.AddModelError("", "Role name is required");
                return View();
            }
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(string id)
        {
            var role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        public ActionResult Edit(IdentityRole role)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    db.Entry(role).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                return View(role);
            }
            catch
            {
                ModelState.AddModelError("", "Enter role name");
                return View(role);
            }
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(string id)
        {

            var role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost]
        public ActionResult Delete(IdentityRole myRole)
        {
            IdentityRole role = db.Roles.Find(myRole.Id);
            db.Roles.Remove(role);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
