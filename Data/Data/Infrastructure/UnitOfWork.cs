using Model.Models;

namespace Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory dbFactory;
        private DbContext dbContext;

        private IRepository<Product> _productRepository;
        private IRepository<ProductCategory> _productCategoryRepository;
        private IRepository<Employee> _employeeRepository;
        private IRepository<Sale> _saleRepository;
        private IRepository<SaleItem> _saleItemRepository;
        private IRepository<Error> _errorRepository;

        public UnitOfWork(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public DbContext DbContext
        {
            get { return dbContext ?? (dbContext = dbFactory.Init()); }
        }


        public IRepository<Product> ProductRepository {
            get { return _productRepository ?? (_productRepository = new Repository<Product>(dbFactory)); }
        }

        public IRepository<ProductCategory> ProductCategoryRepository
        {
            get { return _productCategoryRepository ?? (_productCategoryRepository = new Repository<ProductCategory>(dbFactory)); }
        }

        public IRepository<Error> ErrorRepository
        {
            get { return _errorRepository ?? (_errorRepository = new Repository<Error>(dbFactory)); }
        }

        public IRepository<Employee> EmployeeRepository
        {
            get
            {
                return this._employeeRepository ?? (this._employeeRepository = new Repository<Employee>(dbFactory));
            }
        }

        public IRepository<Sale> SaleRepository
        {
            get
            {
                return this._saleRepository ?? (this._saleRepository = new Repository<Sale>(dbFactory));
            }
        }

        public IRepository<SaleItem> SaleItemRepository
        {
            get
            {
                return this._saleItemRepository ?? (this._saleItemRepository = new Repository<SaleItem>(dbFactory));
            }
        }


        public void Commit()
        {
            DbContext.SaveChanges();
        }
    }
}