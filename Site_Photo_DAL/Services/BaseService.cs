using Microsoft.Extensions.Configuration;
using Site_Photo_DAL.Interface;

namespace Site_Photo_DAL.Interface
{
    public class BaseService<TModel> : IBaseService<TModel>
    {
        protected readonly string _connectionString;

        public BaseService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("default");
        }
    }
}