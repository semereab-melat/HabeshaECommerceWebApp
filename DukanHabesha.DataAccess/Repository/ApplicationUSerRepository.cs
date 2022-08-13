using DukanHabeshaDataAccess.Data;
using DukanHabeshaDataAccess.Repository.IRepository;
using DukanHabeshaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DukanHabeshaDataAccess.Repository;

public class ApplicationUSerRepository : Repository<ApplicationUser>, IApplicationUserRepository
{
    private ApplicationDbContext _db;

    public ApplicationUSerRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
  /*  public void Update(ApplicationUser obj)
    {
        _db.ApplicationUsers.Update(obj);
    }
*/
  
}
