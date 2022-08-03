
using DukanHabeshaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DukanHabeshaDataAccess.Repositories.IRepositories;

public interface ICategoryRepository : IRepository<Category> 
{
    void Update(Category obj);
    
}
