using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DukanHabeshaDataAccess.Repository.IRepository;

public interface IRepository<T> where T : class
{
        //T - Category
        T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true);
        
        //this is similiar with IEnumerable<Catagory> catagories { get; }, except its generic one
        //and can be implemented by all class Models. and it means The Catagory(one of model class)  property will provide
        // read-only access to all the catagories that the application knows about.
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);      
}
