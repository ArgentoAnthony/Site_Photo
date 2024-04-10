using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Site_Photo.Models;
using Site_Photo_DAL.Interface;
using Site_Photo_DAL.Models;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace Site_Photo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPhotoService _photoService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IPhotoService photoService, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _photoService = photoService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Category()
        {
            IEnumerable<string> categories = _photoService.GetAllCategory();
            return View(categories);
        }

        public IActionResult AjoutPhoto()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AjoutPhoto(AddPhotoDTO model)
        {

            if (model.Image != null && model.Image.Length > 0)
            {
                // Générer un nom de fichier unique
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);

                var imagePath = Path.Combine("images", fileName);
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.CopyTo(stream);
                }

                model.ImagePath = imagePath;
                model.DateAjout = DateTime.Now;

                // Enregistrer le modèle dans la base de données
                _photoService.InsertPhoto(model);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(model);
            }
        }
        public IActionResult GetAllPhotos()
        {
            List<string> photoPaths = _photoService.GetAllPhotos();
            return View("ListPhoto", photoPaths);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}