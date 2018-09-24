using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PracenjeTijekaProjekta.Models.PracenjeProjekta
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        [Required]
        [DisplayName("Project name")]
        public string ProjectName { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public String Category { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        [Required]
        [Range(1, 1000)]
        [RegularExpression("([1-9][0-9]*)",ErrorMessage ="Duration is integer.")] 
        [DisplayName("Duration(min)")]
        public int Duration { get; set; }
        [Required]
        [Range(1,1000)]
        [RegularExpression("([1-9][0-9]*)",ErrorMessage = "Number of people needs to be integer.")]
        [DisplayName("People")]
        public int? NumP { get; set; }
    }
}