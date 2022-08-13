﻿
using DukanHabeshaDataAccess.Data;
using DukanHabeshaDataAccess.Repository.IRepository;
using DukanHabeshaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DukanHabeshaDataAccess.Repository;

public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
{
    private ApplicationDbContext _db;

    public ShoppingCartRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public int DecrementCount(ShoppingCart shoppingCart, int Count)
    {
       shoppingCart.Count -=Count;
        return shoppingCart.Count;
    }

    public int IncrementCount(ShoppingCart shoppingCart, int Count)
    {
        shoppingCart.Count += Count;
        return shoppingCart.Count;
    }

}
