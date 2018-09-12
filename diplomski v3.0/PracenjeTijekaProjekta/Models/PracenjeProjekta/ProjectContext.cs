using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PracenjeTijekaProjekta.Models.PracenjeProjekta
{
    public class ProjectContext : DbContext
    {
        public ProjectContext() : base("DefaultConnection") { }

        public DbSet<Categories> Categories { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<UsersProject> UsersProjects { get; set; }

    }
}