using Site_Photo_DAL.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site_Photo_DAL.Interface
{
    public interface IPhotoService
    {
        void CreateCategory(CategoryDTO model);
        void DeleteCategory(int id);
        void DeletePhoto(int id);
        IEnumerable<Category> GetAllCategory();
        List<string> GetAllPhotos( bool largePhoto);
        string GetCategoryNameById(int id);
        int GetPhotoIdByPath(string path);
        List<string> GetPhotoPathsByCategoryId(int id);
        void InsertPhoto(AddPhotoDTO model);
        void UpdateCategory(CategoryDTO model, int id);
    }
}
