using Microsoft.Extensions.Configuration;
using Dapper;
using Site_Photo_DAL.Interface;
using Site_Photo_DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Site_Photo_DAL.Services
{
    public class PhotoService : BaseService<Photo>, IPhotoService
    {
        public PhotoService(IConfiguration config) : base(config)
        {

        }

        public IEnumerable<string> GetAllCategory()
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                IEnumerable<string> categories = connection.Query<string>("SELECT [Name] FROM Category");
                return categories;
            }
        }

        public void InsertPhoto(AddPhotoDTO model)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"INSERT INTO Photos (Name, Image, Id_Category, Date_Ajout) 
                                 VALUES (@Name, @Image, @Category, @DateAjout)";
                connection.Execute(query, model);
            }
        }

        public List<Image> GetAllPhotos()
        {
            List<Image> photos = new List<Image>();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT Image FROM Photos";
                List<byte[]> imagesBytes = connection.Query<byte[]>(query).ToList();
                foreach (byte[] imageData in imagesBytes)
                {
                    using (MemoryStream memoryStream = new MemoryStream(imageData))
                    {
                        Image image = Image.FromStream(memoryStream);
                        photos.Add(image);
                    }
                }
            }
            return photos;
        }

    }

}
