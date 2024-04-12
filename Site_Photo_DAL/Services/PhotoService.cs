using Microsoft.Extensions.Configuration;
using Dapper;
using Site_Photo_DAL.Interface;
using Site_Photo_DAL.Models;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;


namespace Site_Photo_DAL.Services
{
    public class PhotoService : BaseService<Photo>, IPhotoService
    {
        public PhotoService(IConfiguration config) : base(config)
        {
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
                string query = @"INSERT INTO Photos (ImagePath, Id_Category, Date_Ajout) 
                                 VALUES (@ImagePath, @Id_Category, @DateAjout)";
                connection.Execute(query, model);
            }
        } 

        public List<string> GetAllPhotos()
        {
            List<string> photoPaths = new List<string>();
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT TOP 50 ImagePath FROM Photos";
                photoPaths = connection.Query<string>(query).ToList();
            }
            return photoPaths;
        }

        public int GetPhotoIdByPath(string path)
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
