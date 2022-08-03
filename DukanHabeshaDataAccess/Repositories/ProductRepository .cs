using DukanHabeshaDataAccess.Data;
using DukanHabeshaDataAccess.Repositories;
using DukanHabeshaDataAccess.Repositories.IRepositories;
using DukanHabeshaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DukanHabeshaDataAccess.Repositories;


public class ProductRepository : Repository<Product>, IProductRepository
{
    private ApplicationDbContext _db;

    public ProductRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    //we can use our db directly since we are inside
    //our repository, but outside repository, its bad to use our db
    public void Update(Product obj)
    {
        var objFromDb = _db.Products.FirstOrDefault(u => u.Id == obj.Id);
        if(objFromDb != null)
        {
            objFromDb.Id = obj.Id;
            objFromDb.Name = obj.Name;
            objFromDb.Description = obj.Description;
            objFromDb.Price = obj.Price;
            objFromDb.CategoryId = obj.CategoryId;
            objFromDb.OriginId = obj.OriginId;
            objFromDb.IsOnSale = obj.IsOnSale;
            if(obj.ImageUrl != null) 
            { 
                objFromDb.ImageUrl = obj.ImageUrl; 
            }


        }
    }
}
