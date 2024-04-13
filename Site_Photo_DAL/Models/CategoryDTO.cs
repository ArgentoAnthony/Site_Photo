using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site_Photo_DAL.Models
{
    public class CategoryDTO
    {
        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }
    }
}
