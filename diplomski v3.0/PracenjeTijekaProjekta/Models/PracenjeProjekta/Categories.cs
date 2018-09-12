using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PracenjeTijekaProjekta.Models.PracenjeProjekta
{
    public class Categories
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Category name")]
        public string CategoryName { get; set; }
    }
}