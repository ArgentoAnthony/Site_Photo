using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site_Photo_DAL.Models
{
    public class AddPhotoDTO
    {
        [Required(ErrorMessage = "Veuillez sélectionner une image.")]
        public List<IFormFile> Images { get; set; }
        [Required(ErrorMessage = "Veuillez sélectionner une catégorie.")]
        public List<Category> Categories { get; set; }
        public int Id_Category { get; set; }
        public string ImagePath { get; set; }
        public string MiniaturePath { get; set; }
        public DateTime DateAjout { get; set; }
    }
}
