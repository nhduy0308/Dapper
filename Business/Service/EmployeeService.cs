using Data.Infrastructure;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IEmployeeService 
    {
    }

    public class EmployeeService : IEmployeeService
    {
        private IUnitOfWork _unitOfWork { get; set; }
        private IRepository<Employee> _repository { get; set; }
        public EmployeeService(IUnitOfWork unitOfWork)
        {
            this._repository = unitOfWork.EmployeeRepository;
            this._unitOfWork = unitOfWork;
        }
        public Employee Add(Employee _object)
        {
            return this._repository.Add(_object);
        }

        public Employee Delete(int id)
        {
            return this._repository.Delete(id);
        }

        public IEnumerable<Employee> GetAll()
        {
            return this._repository.GetAll();
        }

        public IEnumerable<Employee> GetAll(string keyword = null)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return this._repository.GetMulti(x => x.Name.ToLower().Contains(keyword.ToLower()));
            }
            return this._repository.GetAll();
        }

        public IEnumerable<Employee> GetAllPaging(string keyword = null, int page = 0, int pageSize = 20)
        {
            var query = this._repository.GetMulti(x => x.Name.Contains(keyword));
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }
        public IEnumerable<Employee> GetAllPaging(int page, int pageSize)
        {
            var query = this._repository.GetAll();
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public void Save()
        {
            this._unitOfWork.Commit();
        }


        public void Update(Employee _object)
        {
            this._repository.Update(_object);
            this.Save();
        }
    }
}
