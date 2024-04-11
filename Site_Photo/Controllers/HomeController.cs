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
        [HttpPost]
        public IActionResult DeletePhoto(string PathPhoto)
        {
            int IdPhoto = _photoService.GetPhotoIdByPath(PathPhoto);
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, PathPhoto);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            _photoService.DeletePhoto(IdPhoto);
            return RedirectToAction("GetAllPhotos");
        }
        public IActionResult Category()
        {
            IEnumerable<Category> categories = _photoService.GetAllCategory();
            return View(categories);
        }

        public IActionResult AjoutPhoto()
        {
            var model = new AddPhotoDTO()
            {
                Categories = _photoService.GetAllCategory().ToList()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult AjoutPhoto(AddPhotoDTO model)
        {
            foreach (var image in model.Images)
            {
                if (image != null && image.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    int categoryId = model.Id_Category;
                    var categoryName = _photoService.GetCategoryNameById(categoryId);

                    // Créer le chemin de destination en fonction de la catégorie
                    var webRootPath = _webHostEnvironment.WebRootPath;
                    var categoryPath = Path.Combine("images", categoryName);
                    var imagePath = Path.Combine(categoryPath, fileName);
                    var filePath = Path.Combine(webRootPath, imagePath);

                    if (!Directory.Exists(categoryPath))
                    {
                        Directory.CreateDirectory(categoryPath);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }

                    var photoModel = new AddPhotoDTO
                    {
                        ImagePath = imagePath,
                        Id_Category = categoryId,
                        DateAjout = DateTime.Now
                    };

                    _photoService.InsertPhoto(photoModel);
                }
            }

            return RedirectToAction("Index", "Home");
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