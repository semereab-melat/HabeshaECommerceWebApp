
using DukanHabeshaDataAccess.Data;
using DukanHabeshaDataAccess.Repositories;
using DukanHabeshaDataAccess.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DukanHabeshaDataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            Origin = new OriginRepository(_db);
            
        }
        public ICategoryRepository Category { get; private set; }
         public IProductRepository Product { get; private set; }
         public IOriginRepository Origin { get; private set; }
  
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
