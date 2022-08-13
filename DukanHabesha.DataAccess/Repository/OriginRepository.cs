
using DukanHabeshaDataAccess.Data;
using DukanHabeshaDataAccess.Repository.IRepository;
using DukanHabeshaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DukanHabeshaDataAccess.Repository;

public class OriginRepository : Repository<Origin>, IOriginRepository
{
    private ApplicationDbContext _db;

    public OriginRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(Origin obj)
    {
        _db.Origin.Update(obj);
    }
}
