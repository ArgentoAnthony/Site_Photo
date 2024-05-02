using Microsoft.Extensions.Configuration;
using Dapper;
using Site_Photo_DAL.Interface;
using Site_Photo_DAL.Models;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;


namespace Site_Photo_DAL.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IImageProcessor _imageProcessor;
        protected readonly string _connectionString;
        public PhotoService(IConfiguration config, IImageProcessor imageProcessor)
        {
            _imageProcessor = imageProcessor;
            _connectionString = config.GetConnectionString("default");
        }

        public IEnumerable<Category> GetAllCategory()
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                IEnumerable<Category> categories = connection.Query<Category>("SELECT Id, [Name] FROM Category ORDER BY Id");
                return categories;
            }
        }

        public string GetCategoryNameById(int id) 
        {
            string Name;
            using(IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string Query = $"SELECT [Name] FROM Category WHERE Id = @Id";
                Name = connection.QuerySingleOrDefault<string>(Query, new { Id = id });
            }
            return Name;
        }
        public void CreateCategory(CategoryDTO model)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"INSERT INTO Category (Name) 
                                 VALUES (@Name)";
                connection.Execute(query, model);
            }
        }
        public void UpdateCategory(CategoryDTO model, int id)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"UPDATE Category SET Name = @Name WHERE Id = @Id";
                connection.Execute(query, new { Name = model.Name, Id = id });
            }
        }
        public List<string> GetPhotoPathsByCategoryId(int id)
        {
            List<string> photoPaths = new List<string>();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"SELECT ImagePath FROM Photos P JOIN Category C on C.Id = P.Id_Category WHERE C.Id = @Id";;
                photoPaths = connection.Query<string>(query, new {Id = id}).ToList();
            }
            return photoPaths;
        }
        public List<string> GetMiniaturePathsByCategoryId(int id)
        {
            List<string> photoPaths = new List<string>();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"SELECT MiniaturePath FROM Photos P JOIN Category C on C.Id = P.Id_Category WHERE C.Id = @Id"; ;
                photoPaths = connection.Query<string>(query, new { Id = id }).ToList();
            }
            return photoPaths;
        }
        public void DeleteCategory(int id)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string deletePhotosQuery = "DELETE FROM Photos WHERE Id_Category = @Id";
                connection.Execute(deletePhotosQuery, new { Id = id });
                string Query = "DELETE FROM Category WHERE Id = @Id";
                connection.Execute(Query, new { Id = id });
            }
        }

        public void InsertPhoto(AddPhotoDTO model)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"INSERT INTO Photos (ImagePath,MiniaturePath, Id_Category, Date_Ajout) 
                                 VALUES (@ImagePath,@MiniaturePath, @Id_Category, @DateAjout)";
                connection.Execute(query, model);
            }
        }

        public List<string> GetAllPhotos(int? categoryId = null, bool largePhoto = false)
        {
            List<string> photoPaths = new List<string>();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query;
                if (largePhoto)
                {
                    query = "SELECT ImagePath FROM Photos";
                }
                else
                {
                    query = "SELECT MiniaturePath FROM Photos";
                }

                if (categoryId.HasValue)
                {
                    query += " WHERE Id_Category = @CategoryId";
                }

                photoPaths = connection.Query<string>(query, new { CategoryId = categoryId }).ToList();
            }
            return photoPaths;
        }

        public int GetPhotoIdByMiniaturePath(string path)
        {
            int IdPhoto;
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string Query = $"SELECT Id FROM Photos Where MiniaturePath = @Path";
                IdPhoto = connection.QuerySingleOrDefault<int>(Query, new { Path = path });
            }
            return IdPhoto;
        }
        public int GetPhotoIdByPhotoPath(string path)
        {
            int IdPhoto;
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string Query = $"SELECT Id FROM Photos Where ImagePath = @Path";
                IdPhoto = connection.QuerySingleOrDefault<int>(Query, new { Path = path });
            }
            return IdPhoto;
        }
        public string GetPhotoPathsById(int id)
        {
            string photoPaths;
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"SELECT ImagePath FROM Photos WHERE Id = @Id"; ;
                photoPaths = connection.QuerySingleOrDefault<string>(query, new { Id = id });
            }
            return photoPaths;
        }


        public void DeletePhoto(int id)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string Query = "DELETE FROM Photos WHERE Id = @Id";
                connection.Execute(Query, new { Id = id });
            }
        }

    }
}
