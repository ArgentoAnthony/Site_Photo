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

        public HomeController(ILogger<HomeController> logger, IPhotoService photoService)
        {
            _logger = logger;
            _photoService = photoService;
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
        public IActionResult AjoutPhoto(AddPhotoDTO model, IFormFile image)
        {

                if (image != null && image.Length > 0)
                {
                    // Convertissez l'image en tableau d'octets
                    using (var memoryStream = new MemoryStream())
                    {
                        image.CopyTo(memoryStream);
                        model.Image = memoryStream.ToArray();
                    }
                }
                else
                {
                    return View(model);
                }
                model.DateAjout = DateTime.Now;

                _photoService.InsertPhoto(model);

                return RedirectToAction("Index", "Home");
        }
        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, ImageFormat.Jpeg);
                return memoryStream.ToArray();
            }
        }

        public IActionResult GetAllPhotos()
        {
            IEnumerable<Image> images = _photoService.GetAllPhotos();
            List<byte[]> photos = images.Select(image => ImageToByteArray(image)).ToList();
            return View("ListPhoto", photos);
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