﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Site_Photo.Models;
using Site_Photo_DAL.Interface;
using Site_Photo_DAL.Models;
using System.Diagnostics;
using Site_Photo_DAL.Services;

namespace Site_Photo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPhotoService _photoService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IImageProcessor _imageProcessor;

        public HomeController(ILogger<HomeController> logger, IPhotoService photoService, IWebHostEnvironment webHostEnvironment, IImageProcessor imageProcessor)
        {
            _logger = logger;
            _photoService = photoService;
            _webHostEnvironment = webHostEnvironment;
            _imageProcessor = imageProcessor;
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
                    var miniaturePath = Path.Combine(categoryPath, "miniatures", fileName);
                    var filePath = Path.Combine(webRootPath, imagePath);
                    var miniatureFilePath = Path.Combine(webRootPath, miniaturePath);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(stream);
                    }

                    using (var imageStream = new FileStream(filePath, FileMode.Open))
                    using (var miniatureStream = new FileStream(miniatureFilePath, FileMode.Create))
                    {
                        _imageProcessor.ProcessImage(imageStream, miniatureStream, 200, 200, 80);
                    }

                    var photoModel = new AddPhotoDTO
                    {
                        ImagePath = imagePath,
                        MiniaturePath = miniaturePath,
                        Id_Category = categoryId,
                        DateAjout = DateTime.Now
                    };

                    _photoService.InsertPhoto(photoModel);
                }
            }

            return RedirectToAction("Index", "Home");
        }
        public IActionResult GetAllPhotos(bool largePhoto = false)
        {
            List<string> photoPaths = _photoService.GetAllPhotos(largePhoto);
            return View("ListPhoto", photoPaths);
        }
        public IActionResult ListCategory()
        {
            IEnumerable<Category> categories = _photoService.GetAllCategory();
            return View(categories);
        }
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory(CategoryDTO model)
        {
            _photoService.CreateCategory(model);
            var categoryName = model.Name;
            var webRootPath = _webHostEnvironment.WebRootPath;
            var categoryPath = Path.Combine(webRootPath, "images", categoryName);
            var miniaturePath = Path.Combine(categoryPath, "miniatures");

            if (!Directory.Exists(categoryPath))
            {
                Directory.CreateDirectory(categoryPath);
            }

            if (!Directory.Exists(miniaturePath))
            {
                Directory.CreateDirectory(miniaturePath);
            }

            return RedirectToAction("ListCategory", "Home");
        }
        public IActionResult DeleteCategory(int id)
        {
            var webRootPath = _webHostEnvironment.WebRootPath;
            var categoryPath = Path.Combine(webRootPath, "images", _photoService.GetCategoryNameById(id));

            if (Directory.Exists(categoryPath))
            {
                Directory.Delete(categoryPath, true);
            }
            _photoService.DeleteCategory(id);

            return RedirectToAction("ListCategory", "Home");
        }

        [HttpPost]
        public IActionResult UpdateCategory(int id, string newName)
        {
            var model = new CategoryDTO { Name = newName };
            _photoService.UpdateCategory(model, id);
            return RedirectToAction("ListCategory", "Home");
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