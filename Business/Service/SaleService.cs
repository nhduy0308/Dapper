using Data.Infrastructure;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ISaleService : IService<Sale>
    {

    }
    public class SaleService : ISaleService
    {
        private IUnitOfWork unitOfWork { get; set; }
        private IRepository<Sale> repository { get; set; }
        public SaleService(IUnitOfWork _unitOfWork)
        {
            this.repository = _unitOfWork.SaleRepository;
            this.unitOfWork = _unitOfWork;
            Debug.WriteLine("Initial");
        }
        public Sale Add(Sale _object)
        {
            return this.repository.Add(_object);
        }

        public Sale Delete(int id)
        {
            return this.repository.Delete(id);
        }

        public IEnumerable<Sale> GetAll()
        {
            return this.repository.GetAll();
        }

        public void Save()
        {
            this.unitOfWork.Commit();
        }

        public void Update(Sale _object)
        {
            this.repository.Update(_object);
            this.Save();
        }
    }
}
