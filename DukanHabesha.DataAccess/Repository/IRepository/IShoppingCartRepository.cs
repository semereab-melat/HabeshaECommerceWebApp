using DukanHabeshaModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DukanHabeshaDataAccess.Repository.IRepository;

public interface IShoppingCartRepository : IRepository<ShoppingCart>
{
    int IncrementCount(ShoppingCart shoppingCart, int Count);    
    int DecrementCount(ShoppingCart shoppingCart, int Count);    
}
