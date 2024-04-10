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
        public string? Name { get; set; }
        [Required(ErrorMessage = "Veuillez sélectionner une image.")]
        public byte[] Image { get; set; }
        //[Required]
        public int Category { get; set; }
        public DateTime DateAjout { get; set; }
    }
}
