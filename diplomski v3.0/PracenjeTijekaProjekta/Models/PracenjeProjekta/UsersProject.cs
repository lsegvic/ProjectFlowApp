using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PracenjeTijekaProjekta.Models.PracenjeProjekta
{
    public class UsersProject
    {
        [Key]
        public int ProjectId { get; set; }
        public string UserId { get; set; }

        [Required]
        [DisplayName("Project name")]
        public string ProjectName { get; set; }

        public string ProjectManager { get; set; }

        public DateTime Date {get; set;}
        public string UsersNameSurname { get; set; }    
    }
}