
using DukanHabeshaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DukanHabeshaDataAccess.Repositories.IRepositories;

public interface IOriginRepository : IRepository<Origin>
{
    void Update(Origin obj);
    
}
