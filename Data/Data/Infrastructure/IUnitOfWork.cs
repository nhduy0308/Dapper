using Model.Models;

namespace Data.Infrastructure
{
    public interface IUnitOfWork
    {
        IRepository<Product> ProductRepository { get; }
        IRepository<ProductCategory> ProductCategoryRepository { get; }
        IRepository<Employee> EmployeeRepository { get; }
        IRepository<Sale> SaleRepository { get; }
        IRepository<SaleItem> SaleItemRepository { get; }
        IRepository<Error> ErrorRepository { get; }
        void Commit();
    }
}