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
        void DeletePhoto(int id);
        IEnumerable<Category> GetAllCategory();
        List<string> GetAllPhotos();
        int GetPhotoIdByPath(string path);
        void InsertPhoto(AddPhotoDTO model);
    }
}
