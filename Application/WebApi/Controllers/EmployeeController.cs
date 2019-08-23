using Service;
using System.Web.Http;
using WebApi.Models;
using WebAPI.Infrastructure.Core;

namespace WebApi.Controllers
{
    [RoutePrefix("api/Employee")]
    public class EmployeeController : ApiControllerBase
    {
        private IEmployeeService _employeeService;
        public EmployeeController(IErrorService errorService, IEmployeeService employeeService) : base(errorService)
        {
            this._employeeService = employeeService;
        }

        public IHttpActionResult Get([FromUri]EmployeeGetAllAction ActionCmd)
        {
            ActionCmd._employeeService = this._employeeService;
            return Ok(ActionCmd.Execute());
            
        }
        
    }
}