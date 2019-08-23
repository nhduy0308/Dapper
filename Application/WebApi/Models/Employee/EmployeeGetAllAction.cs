using Model.Models;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Infrastructure.Core;
using WebAPI.Infrastructure.Core;

namespace WebApi.Models
{
    public class EmployeeGetAllAction : CommandBase<IEnumerable<Employee>>
    {
        public IEmployeeService _employeeService { get; set; }
        private IEnumerable<Employee> GetEmployees()
        {
            return this._employeeService.GetAll();
        }
        protected override IResult<IEnumerable<Employee>> ExecuteCore()
        {
            return this.Success(this.GetEmployees());
        }
    }
}