using AutoMapper;
using Model.Models;
using WebApi.Models;
using WebAPI.Models;
namespace WebAPI.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Employee, EmployeeViewModel>();
            });
        }
    }
}